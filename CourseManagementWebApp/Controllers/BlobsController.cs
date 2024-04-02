using AzureStorageLibrary;
using CourseManagementWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementWebApp.Controllers
{
    public class BlobsController : Controller
    {
        private readonly IBlobStorage _blobStorage;
        

        public BlobsController(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<IActionResult> Index()
        {
          var names=   _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl=$"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
             ViewBag.pictures= names.Select(p=>new FileBlob {Name=p,Url=$"{blobUrl}/{p}"}).ToList() ;
            return View();
        }
        public async Task<IActionResult> Upload(IFormFile picture)
        {
            var newFileName=Guid.NewGuid().ToString()+Path.GetExtension(picture.FileName);
            await _blobStorage.UploadAsync(picture.OpenReadStream(), newFileName, EContainerName.pictures);
            await _blobStorage.SetLogAsync("Picture upload was successful","picture_logs.txt");
            TempData["pictures"] = newFileName;
            
            return RedirectToAction("Index", "Courses");
        }
    }
}
