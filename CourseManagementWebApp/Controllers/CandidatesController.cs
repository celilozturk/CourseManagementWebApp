using CourseManagementWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementWebApp.Controllers
{
    public class CandidatesController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Apply(Candidate model)
        {
            return View("Feedback",model);
        }

    }
}
