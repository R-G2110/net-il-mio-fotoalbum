using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_il_mio_fotoalbum.Data
{
    public class PhotoManager
    {
        public static int CountAllPhotos()
        {
            using var db = new PhotoDbContext();
            return db.Photos.Count();
        }

        public static List<Photo> GetAllPhotos(string userId = null)
        {
            using var db = new PhotoDbContext();
            var query = db.Photos.Include(p => p.Categories).AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(p => p.ApplicationUserId == userId);
            }

            return query.ToList();
        }

        public static List<Photo> GetAllVisiblePhotos()
        {
            using var db = new PhotoDbContext();
            return db.Photos.Include(p => p.Categories).Where(p => p.IsVisible).ToList();
        }

        public static List<Photo> GetAllVisiblePhotosByTitle(string title)
        {
            using var db = new PhotoDbContext();
            return db.Photos.Include(p => p.Categories)
                            .Where(p => p.IsVisible && p.Title.Contains(title))
                            .ToList();
        }

        public static async Task<Photo> GetPhotoAsync(int id, bool includeReferences = true)
        {
            using var db = new PhotoDbContext();
            if (includeReferences)
                return await db.Photos.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
            return await db.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public static async Task<Photo> GetPhotoAsync(int id, string userId, bool includeReferences = true)
        {
            using var db = new PhotoDbContext();
            var query = db.Photos.AsQueryable();

            if (includeReferences)
                query = query.Include(p => p.Categories);

            return await query.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == userId);
        }

        public static async Task InsertPhotoAsync(Photo photo, List<string> selectedCategories)
        {
            using var db = new PhotoDbContext();
            photo.Categories = new List<Category>();

            if (selectedCategories != null)
            {
                foreach (var category in selectedCategories)
                {
                    int id = int.Parse(category);
                    var categoryFromDb = await db.Categories.FirstOrDefaultAsync(x => x.Id == id);
                    if (categoryFromDb != null)
                    {
                        photo.Categories.Add(categoryFromDb);
                    }
                }
            }

            db.Photos.Add(photo);
            await db.SaveChangesAsync();
        }

        public static async Task<bool> UpdatePhotoAsync(int id, Photo photo, List<string> selectedCategories, string userId)
        {
            try
            {
                using var db = new PhotoDbContext();
                var existingPhoto = await db.Photos.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);

                if (existingPhoto == null || existingPhoto.ApplicationUserId != userId)
                    return false;

                existingPhoto.Title = photo.Title;
                existingPhoto.Description = photo.Description;
                existingPhoto.ImageFile = photo.ImageFile;
                existingPhoto.IsVisible = photo.IsVisible;

                existingPhoto.Categories.Clear();
                if (selectedCategories != null)
                {
                    foreach (var category in selectedCategories)
                    {
                        int categoryId = int.Parse(category);
                        var categoryFromDb = await db.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
                        if (categoryFromDb != null)
                        {
                            existingPhoto.Categories.Add(categoryFromDb);
                        }
                    }
                }

                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Log exception here (ex)
                return false;
            }
        }

        public static async Task<bool> DeletePhotoAsync(int id, string userId)
        {
            try
            {
                using var db = new PhotoDbContext();
                var photoToDelete = await db.Photos.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == userId);

                if (photoToDelete == null)
                    return false;

                db.Photos.Remove(photoToDelete);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Log exception here (ex)
                return false;
            }
        }

        public static void InsertCategory(Category category)
        {
            using var db = new PhotoDbContext();
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public static List<Category> GetAllCategories()
        {
            using var db = new PhotoDbContext();
            return db.Categories.ToList();
        }

        public static Category GetCategory(int id)
        {
            using var db = new PhotoDbContext();
            return db.Categories.FirstOrDefault(c => c.Id == id);
        }

        public static void UpdateCategory(Category category)
        {
            using var db = new PhotoDbContext();
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static bool DeleteCategory(int id)
        {
            using var db = new PhotoDbContext();
            var category = db.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return false;
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            return true;
        }

        public static void SeedRoles()
        {
            using var context = new PhotoDbContext();
            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Id = "1",
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = DateTime.Now.ToString("yyyy/MM/dd")
                    },
                    new IdentityRole
                    {
                        Id = "21",
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN",
                        ConcurrencyStamp = DateTime.Now.ToString("yyyy/MM/dd")
                    }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }

        public static void SeedUsers()
        {
            using var context = new PhotoDbContext();
            if (!context.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var superAdminUser = new ApplicationUser
                {
                    UserName = "superadmin@superadmin.com",
                    Email = "superadmin@superadmin.com",
                    NormalizedUserName = "SUPERADMIN",
                    NormalizedEmail = "SUPERADMIN@SUPERADMIN.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var passwordHasher = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");
                superAdminUser.PasswordHash = passwordHasher.HashPassword(superAdminUser, "Admin123!");

                context.Users.AddRange(adminUser, superAdminUser);
                context.SaveChanges();

                // Assegna i ruoli agli utenti creati
                var adminRole = new IdentityUserRole<string>
                {
                    UserId = adminUser.Id,
                    RoleId = "1" // Admin role
                };

                var superAdminRole = new IdentityUserRole<string>
                {
                    UserId = superAdminUser.Id,
                    RoleId = "21" // SuperAdmin role
                };

                context.UserRoles.AddRange(adminRole, superAdminRole);
                context.SaveChanges();
            }
        }

        public static void SeedPhotos()
        {
            using var db = new PhotoDbContext();
            var user = db.Users.FirstOrDefault(u => u.UserName == "admin@admin.com");

            if (user != null && !db.Photos.Any())
            {
                db.Photos.AddRange(
                    new Photo
                    {
                        Title = "Sunset",
                        Description = "Beautiful sunset over the hills",
                        IsVisible = true,
                        ApplicationUserId = user.Id
                    },
                    new Photo
                    {
                        Title = "Mountains",
                        Description = "Snow-capped mountains",
                        IsVisible = true,
                        ApplicationUserId = user.Id
                    },
                    new Photo
                    {
                        Title = "Forest",
                        Description = "Dense forest in the morning mist",
                        IsVisible = true,
                        ApplicationUserId = user.Id
                    },
                    new Photo
                    {
                        Title = "Ocean",
                        Description = "Clear blue ocean",
                        IsVisible = true,
                        ApplicationUserId = user.Id
                    },
                    new Photo
                    {
                        Title = "Cityscape",
                        Description = "City skyline at night",
                        IsVisible = true,
                        ApplicationUserId = user.Id
                    }
                );

                db.SaveChanges();
            }
        }

        public static void SeedCategories()
        {
            using var db = new PhotoDbContext();

            if (!db.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Title = "Nature" },
                    new Category { Title = "Urban" },
                    new Category { Title = "Abstract" },
                    new Category { Title = "Portrait" },
                    new Category { Title = "Architecture" }
                };

                db.Categories.AddRange(categories);
                db.SaveChanges();
            }
        }

        public static void SeedData()
        {
            SeedRoles();
            SeedUsers();
            SeedCategories();
            SeedPhotos();
        }
    }
}
