using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordShop.Data.Interfaces;
using WordShop.Models;
using WordShop.Models.DTOs;
using WordShop.Models.Tariff;

namespace WordShop.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ApiController : Controller
    {
        private readonly ICustomerInfoRepository _customerInfoRepository;
        private readonly ITariffBenefitRepository _tariffBenefitRepository;
        private readonly ITariffBenefitOrderedRepository _tariffBenefitOrderedRepository;
        private readonly IDayInfoRepository _dayInfoRepository;
        private readonly IDayInfoBlockRepository _dayInfoBlockRepository;
        private readonly IDayInfoSequenceItemRepository _dayInfoSequenceItemRepository;

        public ApiController(ICustomerInfoRepository customerInfoRepository, 
            ITariffBenefitRepository tariffBenefitRepository,
            ITariffBenefitOrderedRepository tariffBenefitOrderedRepository,
            IDayInfoRepository dayInfoRepository,
            IDayInfoBlockRepository dayInfoBlockRepository,
            IDayInfoSequenceItemRepository dayInfoSequenceItemRepository)
        {
            _customerInfoRepository = customerInfoRepository;
            _tariffBenefitRepository = tariffBenefitRepository;
            _tariffBenefitOrderedRepository = tariffBenefitOrderedRepository;
            _dayInfoRepository = dayInfoRepository;
            _dayInfoBlockRepository = dayInfoBlockRepository;
            _dayInfoSequenceItemRepository = dayInfoSequenceItemRepository;
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
        
        [HttpPost]
        [Route("create-day-info")]
        public async Task<ActionResult> CreateDayInfoAsync([FromBody] DayInfoDto dayInfoDto)
        {
            if (dayInfoDto != null)
            {
                int dayId = await _dayInfoRepository.CreateDayInfoAsync(dayInfoDto);
                
                if (dayInfoDto.BlockInfo.Count > 0)
                {
                    foreach (var blockItem in dayInfoDto.BlockInfo)
                    {
                        int blockId = await _dayInfoBlockRepository.CreateDayInfoBlockAsync(blockItem.BlockTitle, dayId);
                        
                        if(blockItem.Text.Count > 0)
                            await _dayInfoSequenceItemRepository.CreateSequencesFromArrayAsync(blockItem.Text, blockId);
                    }
                }
            }
            return Json(new {success = true, message = "work"}); 
        }
    }
}