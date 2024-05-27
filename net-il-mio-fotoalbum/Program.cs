using Microsoft.AspNetCore.Identity;
using net_il_mio_fotoalbum.Data;
using Microsoft.EntityFrameworkCore;

namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PhotoManager.SeedCategories();
            PhotoManager.SeedPhotos();
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<PhotoDbContext>();
            builder.Services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PhotoDbContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Photo}/{action=PhotoList}/{id?}");
            app.MapRazorPages();
            app.Run();
        }
    }
}