﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            return View(PhotoManager.GetAllPhotos());
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult PhotoList()
        {
            return View(PhotoManager.GetAllPhotos());
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult GetPhoto(int id)
        {
            try
            {
                var photo = PhotoManager.GetPhoto(id);
                if (photo != null)
                    return View(photo);
                else
                    return View("Errore", new ErrorViewModel($"La foto {id} non è stata trovata!"));
            }
            catch (Exception e)
            {
                return View("Errore", new ErrorViewModel(e.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePhoto()
        {
            Photo p = new Photo();
            List<Category> categories = PhotoManager.GetAllCategories();
            var model = new PhotoFormModel(p, categories.Select(c => new SelectListItem { Text = c.Title, Value = c.Id.ToString() }).ToList());
            return View("PhotoForm", model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePhoto(PhotoFormModel photoToInsert)
        {
            if (ModelState.IsValid == false)
            {
                photoToInsert.CreateCategories(); // Aggiornamento delle categorie
                return View("CreatePhoto", photoToInsert);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult UpdatePhoto(int id)
        {
            var photo = PhotoManager.GetPhoto(id);
            if (photo == null)
                return NotFound();
            List<Category> categories = PhotoManager.GetAllCategories();
            var model = new PhotoFormModel(photo, categories.Select(c => new SelectListItem { Text = c.Title, Value = c.Id.ToString() }).ToList());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult UpdatePhoto(int id, PhotoFormModel photoToUpdate)
        {
            if (ModelState.IsValid == false)
            {
                photoToUpdate.CreateCategories(); // Aggiornamento delle categorie
                return View("UpdatePhoto", photoToUpdate);
            }

            var modified = PhotoManager.UpdatePhoto(id, photoToUpdate.Photo);
            if (modified)
            {
                return RedirectToAction("Index");
            }
            else
                return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePhoto(int id)
        {
            var deleted = PhotoManager.DeletePhoto(id);
            if (deleted)
            {
                return RedirectToAction("Index");
            }
            else
                return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}