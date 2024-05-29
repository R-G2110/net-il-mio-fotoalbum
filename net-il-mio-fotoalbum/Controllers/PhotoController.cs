using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly ILogger<PhotoController> _logger;

        public PhotoController(ILogger<PhotoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;
            var photos = PhotoManager.GetAllPhotos(userId);
            return View(photos);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var photo = await PhotoManager.GetPhotoAsync(id, userId);
                if (photo != null)
                    return View("ShowPhoto", photo);
                else
                    return View("Error", new ErrorViewModel($"La foto {id} non è stata trovata!"));
            }
            catch (Exception e)
            {
                return View("Error", new ErrorViewModel(e.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePhoto()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.ApplicationUserId = userId;
            Photo p = new Photo();
            List<Category> categories = PhotoManager.GetAllCategories();
            var model = new PhotoFormModel(p, categories.Select(c => new SelectListItem { Text = c.Title, Value = c.Id.ToString() }).ToList());
            return View("PhotoForm", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePhoto(PhotoFormModel photoToInsert)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.ApplicationUserId = userId;
            photoToInsert.Photo.ApplicationUserId = userId;
            if (!ModelState.IsValid)
            {
                photoToInsert.CreateCategories();
                await PhotoManager.InsertPhotoAsync(photoToInsert.Photo, photoToInsert.SelectedCategories);
                TempData["SuccessMessage"] = "Photo saved successfully.";
                return RedirectToAction("Index");
            }

            return View("PhotoForm", photoToInsert);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdatePhoto(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var photo = await PhotoManager.GetPhotoAsync(id, userId, includeReferences: true);
            if (photo == null)
                return NotFound();

            List<Category> categories = PhotoManager.GetAllCategories();
            var selectedCategoryIds = photo.Categories.Select(c => c.Id.ToString()).ToList();

            PhotoFormModel model = new PhotoFormModel(photo, categories.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString(),
                Selected = selectedCategoryIds.Contains(c.Id.ToString())
            }).ToList());

            return View("PhotoForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> UpdatePhoto(int id, PhotoFormModel photoToUpdate)
        {
            if (ModelState.IsValid)
            {
                photoToUpdate.CreateCategories();
                return View("PhotoForm", photoToUpdate);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var modified = await PhotoManager.UpdatePhotoAsync(id, photoToUpdate.Photo, photoToUpdate.SelectedCategories, userId);
            if (modified)
            {
                TempData["SuccessMessage"] = "Photo modified successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var deleted = await PhotoManager.DeletePhotoAsync(id, userId);
            if (deleted)
            {
                TempData["SuccessMessage"] = "Photo deleted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
