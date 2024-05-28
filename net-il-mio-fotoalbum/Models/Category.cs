using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        [Key] public int Id { get; set; }

        [MinLength(3, ErrorMessage = "The category title must be at least 3 characters long")]
        [Required(ErrorMessage = "The category title field is required")]
        public string Title { get; set; }


        public List<Photo> Photos { get; set; }

    }
}
