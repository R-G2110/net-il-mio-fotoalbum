using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Data
{
    public class PhotoDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }

        public const string CONNECTION_STRING = "Data Source=localhost;Initial Catalog=project_my_photo_album;Integrated Security=True;TrustServerCertificate=True";

        public PhotoDbContext()
        {
        }

        public PhotoDbContext(DbContextOptions<PhotoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Rimuovi il discriminator dalla tabella Photo
            modelBuilder.Entity<Photo>().HasNoDiscriminator();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CONNECTION_STRING);
        }
    }
}
