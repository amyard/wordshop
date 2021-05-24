using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordShop.Data.Interfaces;

namespace WordShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly ICustomerInfoRepository _customerInfoRepository;

        public AdminController(ICustomerInfoRepository customerInfoRepository)
        {
            _customerInfoRepository = customerInfoRepository;
        }
        
        [HttpGet]
        [Route("get-customer-info")]
        public async Task<IActionResult> GetCustomerInformation()
        {
            var res = await _customerInfoRepository.GetAllCustomersAsync();
            return Json(new { data = res });
        }
    }
}