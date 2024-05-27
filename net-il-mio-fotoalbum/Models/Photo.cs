using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Il titolo deve avere al più 50 caratteri")]
        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public string Title { get; set; }

        [MinLength(10, ErrorMessage = "La descrizione deve avere almeno 10 caratteri")]
        public string Description { get; set; }

        public byte[]? ImageFile { get; set; }
        public string ImgSrc => ImageFile != null ? $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}" : "";

        public bool IsVisible { get; set; }

        public DateTime Timestamp { get; set; }

        public List<Category>? Categories { get; set; }

        public Photo()
        {
            Timestamp = DateTime.Now;
            Categories = new List<Category>();
        }

        public Photo(string title, string description) : this()
        {
            Title = title;
            Description = description;
        }

        public string GetDisplayedCategory()
        {
            return Categories != null && Categories.Count > 0
                ? string.Join(", ", Categories.Select(c => c.Title))
                : "Nessuna categoria";
        }
    }
}
