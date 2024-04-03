using AzureStorageLibrary;
using CourseManagementWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseManagementWebApp.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly INoSqlStorage<Candidate> _tableStorageCandidate;
        private readonly INoSqlStorage<Course> _tableStorageCourse;

        public CandidatesController(INoSqlStorage<Course> tableStorageCourse, INoSqlStorage<Candidate> tableStorageCandidate)
        {
            _tableStorageCourse = tableStorageCourse;
            _tableStorageCandidate = tableStorageCandidate;
        }

        public IActionResult Index()
        {
           var courses= _tableStorageCourse.GetAll().ToList();
            ViewBag.Courses = new SelectList(courses, "Name","Name");
          //  ViewBag.Courses = new SelectList(courses, "RowKey","Name");
            
            return View();
        }
        public IActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Apply(Candidate model)
        {
            model.RowKey=Guid.NewGuid().ToString();
            model.PartitionKey = model.Email;
            _tableStorageCandidate.AddAsync(model);
            
            return View("Feedback",model);
        }

    }
}
