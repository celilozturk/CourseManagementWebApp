using AzureStorageLibrary;
using CourseManagementWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.OData.Edm;

namespace CourseManagementWebApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly INoSqlStorage<Course> _tableStorage;

        public CoursesController(INoSqlStorage<Course> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public IActionResult Index()
        {
            ViewBag.courses = _tableStorage.GetAll().ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {

            course.RowKey = Guid.NewGuid().ToString();
            course.PartitionKey = course.CourseCategory.ToString();
            course.TotalParticipant = int.MinValue;
            await _tableStorage.AddAsync(course);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string rowkey, string partitionKey)
        {
            await _tableStorage.DeleteAsync(rowkey, partitionKey);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult FilterByName(string filteredName)
        {
            if (!string.IsNullOrEmpty(filteredName))
            {
                var filteredCourses = _tableStorage.Query(c => c.Name.Equals(filteredName)).ToList();
                ViewBag.courses = filteredCourses;
                return View("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
