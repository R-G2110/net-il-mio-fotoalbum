using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<string> SelectedCategories { get; set; }
        public IFormFile ImageFormFile { get; set; }

        public PhotoFormModel(Photo photo, List<SelectListItem> categories)
        {
            Photo = photo;
            Categories = categories;
            SelectedCategories = new List<string>();
        }

        public void CreateCategories()
        {
            SelectedCategories ??= new List<string>();
            foreach (var category in Categories)
            {
                bool isSelected = SelectedCategories.Contains(category.Value);
                category.Selected = isSelected;
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
