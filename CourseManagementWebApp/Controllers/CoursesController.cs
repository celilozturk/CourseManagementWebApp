using Microsoft.AspNetCore.Mvc;

namespace CourseManagementWebApp.Controllers
{
    public class CoursesController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
