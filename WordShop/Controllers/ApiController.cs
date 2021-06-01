using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordShop.Data.Interfaces;

namespace WordShop.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ApiController : Controller
    {
        private readonly ICustomerInfoRepository _customerInfoRepository;
        private readonly ITariffBenefitRepository _tariffBenefitRepository;

        public ApiController(ICustomerInfoRepository customerInfoRepository, 
            ITariffBenefitRepository tariffBenefitRepository)
        {
            _customerInfoRepository = customerInfoRepository;
            _tariffBenefitRepository = tariffBenefitRepository;
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
    }
}