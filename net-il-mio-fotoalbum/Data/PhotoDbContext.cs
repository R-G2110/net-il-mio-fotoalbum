using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
    public class PhotoDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public PhotoDbContext()
        {
        }

        public PhotoDbContext(DbContextOptions<PhotoDbContext> options) : base(options)
        {
        }
        public const string CONNECTION_STRING = "Data Source=localhost;Initial Catalog=project_photo_album;Integrated Security=True;TrustServerCertificate=True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CONNECTION_STRING);
        }
    }
}
