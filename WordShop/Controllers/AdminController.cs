using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordShop.Data.Interfaces;
using WordShop.Models;

namespace WordShop.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : Controller
    {
        private readonly ICourseStartRepository _courseStartRepository;
        private readonly ITariffBenefitRepository _tariffBenefitRepository;

        public AdminController(ICourseStartRepository courseStartRepository,
            ITariffBenefitRepository tariffBenefitRepository)
        {
            _courseStartRepository = courseStartRepository;
            _tariffBenefitRepository = tariffBenefitRepository;
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
    }
}