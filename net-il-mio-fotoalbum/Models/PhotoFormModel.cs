using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using net_il_mio_fotoalbum.Data;
using System.IO;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<string> SelectedCategories { get; set; }
        public IFormFile ImageFormFile { get; set; }
        public string ApplicationUserId { get; internal set; }

        public PhotoFormModel()
        {
            // Costruttore vuoto
        }

        public PhotoFormModel(Photo photo, List<SelectListItem> categories)
        {
            Photo = photo;
            Categories = categories;
            SelectedCategories = new List<string>();
        }

        public PhotoFormModel(Photo photo)
        {
            Photo = photo;
        }

        public void CreateCategories()
        {
            this.Categories = new List<SelectListItem>();
            if (this.SelectedCategories == null)
                this.SelectedCategories = new List<string>();
            var categoriesFromDB = PhotoManager.GetAllCategories();
            foreach (var category in categoriesFromDB) // tutti gli ingredienti possibili: i1, i2, i3, ... i10
            {
                bool isSelected = this.SelectedCategories.Contains(category.Id.ToString()); // this.Pizza.Ingredients?.Any(i => i.Id == ingrendient.Id) == true;
                this.Categories.Add(new SelectListItem() // lista degli elementi selezionabili
                {
                    Text = category.Title, // Testo visualizzato
                    Value = category.Id.ToString(), // SelectListItem vuole una generica stringa, non un int
                    Selected = isSelected // es. i1, i5, i9
                });
                  this.SelectedCategories.Add(category.Id.ToString()); // lista degli elementi selezionati
            }
        }
        

        public byte[] SetImageFileFromFormFile()
        {
            if (ImageFormFile == null)
                return null;

            using var stream = new MemoryStream();
            ImageFormFile.CopyTo(stream);
            Photo.ImageFile = stream.ToArray();

            return Photo.ImageFile;
        }
    }
}
