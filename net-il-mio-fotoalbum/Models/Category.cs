using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }

        public List<Photo> Photos { get; set; }

        public Category() { }
        public Category(string title) : this()
        {
            Title = title;
        }
    }
}
