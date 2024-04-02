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
        private readonly IBlobStorage _blobStorage;

        public CoursesController(INoSqlStorage<Course> tableStorage, IBlobStorage blobStorage)
        {
            _tableStorage = tableStorage;
            _blobStorage = blobStorage;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.courses = _tableStorage.GetAll().ToList();
            ViewBag.isUpdate = false;
            ViewBag.logs = await _blobStorage.GetLogAsync("course_logs.txt");
            ViewBag.blobUrl = _blobStorage.BlobUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Course course,IFormFile picture)
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
            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
            await _blobStorage.UploadAsync(picture.OpenReadStream(), newFileName,EContainerName.pictures);
            course.CoursePicture = newFileName;
            await _tableStorage.AddAsync(course);
            await _blobStorage.SetLogAsync("New Course was added!", "course_logs.txt");
            ViewBag.courses = _tableStorage.GetAll().ToList();
            ViewBag.blobUrl = _blobStorage.BlobUrl;
            return RedirectToAction("Index");
          
        }
        [HttpGet]
        public async Task<IActionResult> Update(string rowkey, string partitionKey)
        {
            var course=await _tableStorage.GetAsync(rowkey, partitionKey);
            ViewBag.courses = _tableStorage.GetAll().ToList();
            ViewBag.isUpdate = true;
            ViewBag.logs = await _blobStorage.GetLogAsync("course_logs.txt");
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
            await _blobStorage.SetLogAsync("New Course was updated successfully!", "course_logs.txt");
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string rowkey, string partitionKey)
        {
            await _tableStorage.DeleteAsync(rowkey, partitionKey);
            await _blobStorage.SetLogAsync("New Course was deleted successfully!", "course_logs.txt");
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
