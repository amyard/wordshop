using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models;

namespace WordShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerInfoRepository _customerInfoRepository;
        private readonly ITariffRepository _tariffRepository;
        
        private const string emailPattern  = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public HomeController(ILogger<HomeController> logger, ICustomerInfoRepository customerInfoRepository,
            ITariffRepository tariffRepository)
        {
            _logger = logger;
            _customerInfoRepository = customerInfoRepository;
            _tariffRepository = tariffRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _tariffRepository.GetAllTariffsAsync());
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Route("save-customer-info")]
        public async Task<ActionResult> SaveCustomerInfoAsync([FromBody] CustomerInfoRequest customerInfoRequest)
        {
            const Courses course = Courses.WordShop;
            const Level level = Level.Beginner;
            
            // name validation
            if (string.IsNullOrWhiteSpace(customerInfoRequest.FullName))
                return Json(new {success = false, message = "Имя не может быть пустым", error="standard"});
            
            if (customerInfoRequest.FullName.Length < 3)
                return Json(new {success = false, message = "Имя должно быть более 3-х символов", error="standard"});
            
            // email validation
            if (string.IsNullOrWhiteSpace(customerInfoRequest.Email))
                return Json(new {success = false, message = "Адрес не может быть пустым", error="standard"});
            
            if (customerInfoRequest.Email.Length < 5)
                return Json(new {success = false, message = "Адрес должно быть более 5-ти символов", error="standard"});
            
            if (!IsEmailValid(customerInfoRequest.Email))
                return Json(new {success = false, message = "Адрес должен быть вида name@domain.com", error="standard"});
            
            if (await _customerInfoRepository.IsEmailUniqueByCourseAndTariff(customerInfoRequest.Email, course, level, customerInfoRequest.TariffId))
                return Json(new {success = false, message = "Данный адрес уже используется", error="standard"});
            
            // tariff validation
            if (string.IsNullOrWhiteSpace(customerInfoRequest.TariffId.ToString()))
                return Json(new {success = false, message = "Выберите тариф", error="standard"});
            
            if(!await _tariffRepository.IsTariffExists(customerInfoRequest.TariffId, course, level))
                return Json(new {success = false, message = "Данного тарифа не существует", error="unexpected"});
            
            // save data
            // TODO -> move to file
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
            
            if (await _customerInfoRepository.SaveAllAsync())
                return Json(new {success = true, message = "work"}); 
            
            // payment process
            // telegram notification + email
            // check on exception
            return Json(new {success = false, message = "some error", error="unexpected"});
        }

        private bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
        }
    }
}
