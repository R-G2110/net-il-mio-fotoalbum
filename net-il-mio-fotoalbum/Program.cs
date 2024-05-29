using Microsoft.AspNetCore.Identity;
using net_il_mio_fotoalbum.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using net_il_mio_fotoalbum.Models;


namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PhotoManager.SeedData();
            
            var builder = WebApplication.CreateBuilder(args);
                     //   var connectionString = builder.Configuration.GetConnectionString("NOMEVOSTROCONTEXTConnection") ?? throw new InvalidOperationException("Connection string 'NOMEVOSTROCONTEXTConnection' not found.");
            builder.Services.AddDbContext<PhotoDbContext>();
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<PhotoDbContext>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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
                pattern: "{controller=Guest}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
        }
    }
}