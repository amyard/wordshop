using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models;
using WordShop.Models.CustomerInfo;
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

        private const Courses course = Courses.WordShop;
        private const Level level = Level.Beginner;
        
        public HomeController(ILogger<HomeController> logger, ICustomerInfoRepository customerInfoRepository,
            ITariffRepository tariffRepository, ITelegramRepository telegram, 
            ICourseStartRepository courseStartRepository,
            IDayInfoRepository dayInfoRepository)
        {
            _logger = logger;
            _customerInfoRepository = customerInfoRepository;
            _tariffRepository = tariffRepository;
            _telegram = telegram;
            _courseStartRepository = courseStartRepository;
            _dayInfoRepository = dayInfoRepository;
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

        [HttpPost]
        [Route("save-customer-info")]
        public async Task<ActionResult> SaveCustomerInfoAsync([FromBody] CustomerInfoRequest customerInfoRequest)
        {
            if (!ModelState.IsValid)
                return Json( GetModelInvalidError(ModelState) );

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
                CourseLevel = level
            };
            
            await _customerInfoRepository.SaveCustomerInfoAsync(customer);
            await _customerInfoRepository.SaveAllAsync();
            
            // payment process
            
            // telegram notification + email
            customer.Tariff = await _tariffRepository.GetTariffByIdAsync(customer.TariffId);
            await _telegram.SendNewCustomerMessageToGroup(customer);

            return Json(new {success = true, message = "work"}); 
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        #endregion

        # region private methods
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
