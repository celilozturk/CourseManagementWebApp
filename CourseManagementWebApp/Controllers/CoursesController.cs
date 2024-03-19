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
            ViewBag.isUpdate = false;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (course.Name ==null)
            {
                //ModelState.AddModelError("Name","Name field is required!");
                ViewBag.isUpdate = false;
                ViewBag.courses = _tableStorage.GetAll().ToList();
                return View("Index");
            }
            course.RowKey = Guid.NewGuid().ToString();
            course.PartitionKey = course.CourseCategory.ToString();
            course.TotalParticipant = 0;
            await _tableStorage.AddAsync(course);
            ViewBag.courses = _tableStorage.GetAll().ToList();
            return RedirectToAction("Index");
          
        }
        [HttpGet]
        public async Task<IActionResult> Update(string rowkey, string partitionKey)
        {
            var course=await _tableStorage.GetAsync(rowkey, partitionKey);
            ViewBag.courses = _tableStorage.GetAll().ToList();
            ViewBag.isUpdate = true;
            return View("Index",course);

        }

            [HttpPost]
        public async Task<IActionResult> Update(Course course)
        {
            if (course.Name == null)
            {
                //ModelState.AddModelError("Name","Name field is required!");
                ViewBag.isUpdate = false;
                ViewBag.courses = _tableStorage.GetAll().ToList();
                return View("Index");
            }
            course.ETag = "*";
          
            ViewBag.isUpdate=false;
            await _tableStorage.UpdateAsync(course);
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
                var filteredCourses = _tableStorage.Query( x=> x.Name.Equals(filteredName)).ToList();
                ViewBag.courses = filteredCourses.ToList();
                ViewBag.isUpdate = false;
                return View("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
