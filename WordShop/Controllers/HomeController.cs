using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Enums.Payment;
using WordShop.Models;
using WordShop.Models.CustomerInfo;
using WordShop.Models.Payment;
using WordShop.Models.Settings;
using WordShop.Models.ViewModels;

namespace WordShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerInfoRepository _customerInfoRepository;
        private readonly ITariffRepository _tariffRepository;
        private readonly ITelegramRepository _telegram;
        private readonly ICourseStartRepository _courseStartRepository;
        private readonly IDayInfoRepository _dayInfoRepository;
        private readonly IOptions<LiqPaySettingsModel> _paymentSettings;

        private const Courses course = Courses.WordShop;
        private const Level level = Level.Beginner;
        
        public HomeController(ILogger<HomeController> logger, 
            ICustomerInfoRepository customerInfoRepository,
            ITariffRepository tariffRepository, 
            ITelegramRepository telegram, 
            ICourseStartRepository courseStartRepository,
            IDayInfoRepository dayInfoRepository,
            IOptions<LiqPaySettingsModel> paymentSettings)
        {
            _logger = logger;
            _customerInfoRepository = customerInfoRepository;
            _tariffRepository = tariffRepository;
            _telegram = telegram;
            _courseStartRepository = courseStartRepository;
            _dayInfoRepository = dayInfoRepository;
            _paymentSettings = paymentSettings;
        }

        # region public methods
        
        public async Task<IActionResult> Index()
        {
            CultureInfo ruRu = new CultureInfo("ru-RU");
            
            var startDateFromDB = _courseStartRepository.GetCourseStart();
            var startDate = startDateFromDB != null
                ? startDateFromDB.CourseStartDate.ToString("dd MMMM yyyy", ruRu)
                : DateTime.Now.ToString("dd MMMM yyyy", ruRu);
            
            IndexViewModel result = new IndexViewModel
            {
                CourseStart = startDate,
                Tariffs = await _tariffRepository.GetAllTariffsAsync(course, level),
                DayInfos = await _dayInfoRepository.GetListOfDayInfosAsync()
            };
            
            return View(result);
        }

        [Route("privacy-policy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [HttpPost]
        [Route("save-customer-info")]
        public async Task<ActionResult> SaveCustomerInfoAsync([FromBody] CustomerInfoRequest customerInfoRequest)
        {
            if (!ModelState.IsValid)
                return Json(GetModelInvalidError(ModelState));

            JsonResponseResult customValidation = CheckModelValidation(customerInfoRequest);

            if (!customValidation.Success)
                return Json(customValidation);

            // save data
            CustomerInfo customer = new CustomerInfo
            {
                FullName = customerInfoRequest.FullName,
                Email = customerInfoRequest.Email.ToLower(),
                PhoneNumber = customerInfoRequest.PhoneNumber,
                TariffId = customerInfoRequest.TariffId,
                Courses = course,
                CourseLevel = level,
                OrderId = Guid.NewGuid()
            };

            await _customerInfoRepository.SaveCustomerInfoAsync(customer);
            await _customerInfoRepository.SaveAllAsync();

            // payment process
            customer.Tariff = await _tariffRepository.GetTariffByIdAsync(customer.TariffId);
            string paymentFullUrl = GeneratePaymentFullUrl(customer);

            // telegram notification + email
            await _telegram.SendNewCustomerMessageToGroup(customer);

            return Json(new {success = true, message = "work", redirectUrl = paymentFullUrl});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        #endregion

        # region private methods
        
        private string GeneratePaymentFullUrl(CustomerInfo customer)
        {
            // TODO ---> override to redirect on website url with order id
            string resultUrl = "https://www.google.com/";
            string orderId = customer.OrderId.ToString();
            
            var paymentEntity = new LiqPayRequest
            {
                Version = 3,
                PublicKey = _paymentSettings.Value.ApiPublicKey,
                Action = LiqPayRequestAction.Pay,
                Amount = Convert.ToDouble(customer.Tariff.NewPrice),
                Currency = LiqPayCurrency.UAH,
                Description = $"Оплата курсу {course} по тарифу {customer.Tariff.Name}",
                OrderId = orderId,
                Language = LiqPayRequestLanguage.UK,
                ResultUrl = resultUrl
            };

            // 1. convert to base64_encode the model
            var jsonString = JObject.FromObject(paymentEntity,
                new JsonSerializer { NullValueHandling = NullValueHandling.Ignore })
                .ToString();
            
            byte[] textAsBytes = Encoding.UTF8.GetBytes(jsonString);
            string data = Convert.ToBase64String(textAsBytes);
            
            // 2. create signature - concatenate private_key + data + private_key
            // 3. base64_encode( sha1( sign_string) )
            var stringToHash = string.Concat(_paymentSettings.Value.ApiPrivateKey, data, _paymentSettings.Value.ApiPrivateKey);
            byte[] sha1 = SHA1Hash(stringToHash);
            string signature = Convert.ToBase64String(sha1);
            
            // 4. result url
            var result = string.Concat(
                _paymentSettings.Value.CheckOutUrl,
                "?data=", data,
                "&signature=", signature
            );

            return result;
        }

        private byte[] SHA1Hash(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            }
        }

        private JsonResponseResult GetModelInvalidError(ModelStateDictionary modelState)
        {
            // TODO --> override to reflection and take ordering by model column name.
            List<string> orderedList = new List<string>(new string[] {"FullName", "Email"});

            var entity = modelState
                .Select(m => new {Order = orderedList.IndexOf(m.Key), Error = m.Value})
                .OrderBy(m => m.Order)
                .FirstOrDefault();

            JsonResponseResult result = new JsonResponseResult
            {
                Success = false,
                Message = entity.Error.Errors.FirstOrDefault().ErrorMessage,
                Error = ErrorTypes.Standard
            };
            
            return result;
        }

        private JsonResponseResult CheckModelValidation(CustomerInfoRequest customerInfoRequest)
        {
            bool success = true;
            string message = "";
            ErrorTypes error = ErrorTypes.None;
            
            if (_customerInfoRepository
                .IsEmailUniqueByCourseAndTariff(customerInfoRequest.Email, course, level, customerInfoRequest.TariffId)
                .Result)
            {
                (success, message, error) = (false, "Данный адрес уже используется", ErrorTypes.Standard);
            }

            if (!_tariffRepository.IsTariffExists(customerInfoRequest.TariffId, course, level).Result)
            {
                (success, message, error) = (false, "Данного тарифа не существует", ErrorTypes.Unexpected);
            }
            
            JsonResponseResult result = new JsonResponseResult
            {
                Success = success,
                Message = message,
                Error = error
            };
            
            return result;
        }
        
        # endregion
    }
}
