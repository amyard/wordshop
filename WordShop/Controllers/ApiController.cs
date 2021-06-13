using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordShop.Data.Interfaces;
using WordShop.Models;

namespace WordShop.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ApiController : Controller
    {
        private readonly ICustomerInfoRepository _customerInfoRepository;
        private readonly ITariffBenefitRepository _tariffBenefitRepository;
        private readonly ITariffBenefitOrderedRepository _tariffBenefitOrderedRepository;

        public ApiController(ICustomerInfoRepository customerInfoRepository, 
            ITariffBenefitRepository tariffBenefitRepository,
            ITariffBenefitOrderedRepository tariffBenefitOrderedRepository)
        {
            _customerInfoRepository = customerInfoRepository;
            _tariffBenefitRepository = tariffBenefitRepository;
            _tariffBenefitOrderedRepository = tariffBenefitOrderedRepository;
        }
        
        [HttpGet]
        [Route("get-customer-info")]
        public async Task<IActionResult> GetCustomerInformation()
        {
            return Json(new { data = await _customerInfoRepository.GetAllCustomersAsync() });
        }
        
        [HttpGet]
        [Route("get-tariff-benefits")]
        public async Task<IActionResult> GetTariffBenefits()
        {
            return Json(new { data = await _tariffBenefitRepository.GetTariffBenefitsList() });
        }
        
        [HttpPost]
        [Route("save-tariff-benefit/{tariffId:int}")]
        public async Task<ActionResult> SaveCustomerInfoAsync([FromBody] List<TariffBenefitOrdered> data, int tariffId)
        {
            if (data != null)
            {
                await _tariffBenefitOrderedRepository.DeleteBenefitByTariffId(tariffId);
                await _tariffBenefitOrderedRepository.SaveTariffBenefitRange(data);
                await _tariffBenefitOrderedRepository.SaveAllAsync();
            }
            return Json(new {success = true, message = "work"}); 
        }
    }
}