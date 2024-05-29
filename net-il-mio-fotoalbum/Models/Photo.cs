using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "The title must be at most 50 characters long")]
        [Required(ErrorMessage = "The title field is required")]
        public string Title { get; set; }

        [MinLength(10, ErrorMessage = "The description must be at least 10 characters long")]
        [Required(ErrorMessage = "The description field is required")]
        public string? Description { get; set; }

        public byte[]? ImageFile { get; set; }
        public string ImgSrc => ImageFile != null ? $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}" : "";

        public bool IsVisible { get; set; }

        // Foreign key for ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime Timestamp { get; set; }

        public List<Category>? Categories { get; set; }

        public Photo()
        {
            Timestamp = DateTime.Now;
        }

        public Photo(string title, string? description) : this()
        {
            Title = title;
            Description = description;
        }

        
    }
}
