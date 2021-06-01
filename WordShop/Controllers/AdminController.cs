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

        public AdminController(ICourseStartRepository courseStartRepository)
        {
            _courseStartRepository = courseStartRepository;
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
    }
}