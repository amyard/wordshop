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

        public ApiController(ICustomerInfoRepository customerInfoRepository)
        {
            _customerInfoRepository = customerInfoRepository;
        }
        
        [HttpGet]
        [Route("get-customer-info")]
        public async Task<IActionResult> GetCustomerInformation()
        {
            return Json(new { data = await _customerInfoRepository.GetAllCustomersAsync() });
        }
    }
}