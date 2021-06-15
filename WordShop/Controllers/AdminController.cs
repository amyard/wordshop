using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models;
using WordShop.Models.DayInfo;
using WordShop.Models.Tariff;
using WordShop.Models.ViewModels;

namespace WordShop.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : Controller
    {
        private const Courses course = Courses.WordShop;
        private const Level level = Level.Beginner;
        
        
        private readonly ICourseStartRepository _courseStartRepository;
        private readonly ITariffBenefitRepository _tariffBenefitRepository;
        private readonly ITariffRepository _tariffRepository;
        private readonly ITariffBenefitOrderedRepository _tariffBenefitOrderedRepository;
        private readonly IDayInfoRepository _dayInfoRepository;

        public AdminController(ICourseStartRepository courseStartRepository,
            ITariffBenefitRepository tariffBenefitRepository,
            ITariffRepository tariffRepository,
            ITariffBenefitOrderedRepository tariffBenefitOrderedRepository,
            IDayInfoRepository dayInfoRepository)
        {
            _courseStartRepository = courseStartRepository;
            _tariffBenefitRepository = tariffBenefitRepository;
            _tariffRepository = tariffRepository;
            _tariffBenefitOrderedRepository = tariffBenefitOrderedRepository;
            _dayInfoRepository = dayInfoRepository;
        }
        
        [Route("dashboard")]
        public IActionResult AdminDashboard()
        {
            return View();
        }
        
        [Route("course-start")]
        public IActionResult AdminCourseStart()
        {
            return View(_courseStartRepository.GetCourseStart());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminCourseStartUpdate(CourseStart courseStart)
        {
            if (ModelState.IsValid)
            {
                _courseStartRepository.UpdateCourseStartAsync(courseStart);
            }
            return RedirectToAction("AdminDashboard");
        }

        #region TariffBenefitt
        
        [Route("tariff-benefit")]
        public IActionResult AdminTariffBenefit()
        {
            return View();
        }

        [HttpGet]
        [Route("tariff-benefit-action")]
        public IActionResult AdminTariffBenefitToggle(int? id)
        {
            TariffBenefit benefit = id.HasValue
                ? _tariffBenefitRepository.GetTariffBenefit((int)id).Result
                : new TariffBenefit();
            
            return View(benefit);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminTariffBenefitAction(TariffBenefit tariffBenefit)
        {
            if (ModelState.IsValid)
            {
                if (tariffBenefit.Id == 0)
                    await _tariffBenefitRepository.CreateTariffBenefit(tariffBenefit);
                else
                    await _tariffBenefitRepository.UpdateTariffBenefit(tariffBenefit);
                
                await _tariffBenefitRepository.SaveAllAsync();
            }
            return RedirectToAction(nameof(AdminTariffBenefit));
        }
        
        #endregion
        

        #region TariffAction
        
        [Route("tariff")]
        public async Task<IActionResult> AdminTariff()
        {
            return View(await _tariffRepository.GetAllTariffsAsync(course, level));
        }
        
        [HttpGet]
        [Route("tariff-action")]
        public async Task<IActionResult> AdminTariffToggle(int? id)
        {
            Tariff tariff = id.HasValue
                ? await _tariffRepository.GetTariffByIdAsync((int)id)
                : new Tariff();

            var tariffbenefit = await _tariffBenefitRepository.GetTariffBenefitsList();
            
            if (id.HasValue)
            {
                var benefitsInUse = await _tariffBenefitOrderedRepository.GetBenefitsByTariffId((int)id);
                tariffbenefit = await _tariffBenefitRepository.GetTariffBenefitsListWithoutInUseIds(benefitsInUse);
            }

            var result = new TariffViewModel
            {
                Tariff = tariff,
                TariffBenefits = tariffbenefit.ToList()
            };
            
            return View(result);
        } 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminTariffAction(Tariff tariff)
        {
            if (ModelState.IsValid)
            {
                await _tariffRepository.UpdateTariffAsync(tariff);
                
                await _tariffRepository.SaveAllAsync();
            }
            return RedirectToAction(nameof(AdminTariff));
        }
        #endregion


        #region  dayinfo
        public async Task<ActionResult> AdminDayInfo()
        {
            return View(await _dayInfoRepository.GetListOfDayInfosAsync());
        }
        #endregion
    }
}