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
        private readonly IBlobStorage _blobStorage;

        public CandidatesController(INoSqlStorage<Course> tableStorageCourse, INoSqlStorage<Candidate> tableStorageCandidate, IBlobStorage blobStorage)
        {
            _tableStorageCourse = tableStorageCourse;
            _tableStorageCandidate = tableStorageCandidate;
            _blobStorage = blobStorage;
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
        public async Task<IActionResult> Apply(Candidate model)
        {
            model.RowKey=Guid.NewGuid().ToString();
            model.PartitionKey = model.Email;
            _tableStorageCandidate.AddAsync(model);
            _blobStorage.SetLogAsync("New candidate application was created!", "candidate_application_logs.txt");
            TempData["logs"] =await _blobStorage.GetLogAsync("candidate_application_logs.txt");
            TempData.Keep();
            return View("Feedback",model);
        }

    }
}
