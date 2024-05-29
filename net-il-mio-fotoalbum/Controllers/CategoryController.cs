using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data; // Assicurati di avere lo spazio dei nomi corretto per PhotoManager
using net_il_mio_fotoalbum.Models;
using System;
using System.Collections.Generic;

namespace net_il_mio_fotoalbum.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            List<Category> categories = PhotoManager.GetAllCategories();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View("CategoryForm", new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title")] Category category)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    PhotoManager.InsertCategory(category);
                    TempData["SuccessMessage"] = "Category created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "An error occurred while creating the category.";
                    return View("CategoryForm", category);
                }
            }
            TempData["ErrorMessage"] = "There was a problem with the provided data.";
            return View("CategoryForm", category);
        }

        public IActionResult Edit(int id)
        {
            Category category = PhotoManager.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View("CategoryForm", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    PhotoManager.UpdateCategory(category);
                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the category.";
                    return View("CategoryForm", category);
                }
            }
            TempData["ErrorMessage"] = "There was a problem with the provided data.";
            return View("CategoryForm", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                PhotoManager.DeleteCategory(id);
                TempData["SuccessMessage"] = "Category deleted successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the category.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
