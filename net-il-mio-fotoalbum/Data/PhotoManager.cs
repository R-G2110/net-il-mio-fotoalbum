using net_il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace net_il_mio_fotoalbum.Data
{
    public enum ResultType
    {
        OK,
        Exception,
        NotFound
    }

    public class PhotoManager
    {
        public static int CountAllPhotos()
        {
            using PhotoDbContext db = new PhotoDbContext();
            return db.Photos.Count();
        }

        public static List<Photo> GetAllPhotos()
        {
            using PhotoDbContext db = new PhotoDbContext();
            return db.Photos.ToList();
        }

        public static Photo GetPhoto(int id, bool includeReferences = true)
        {
            using PhotoDbContext db = new PhotoDbContext();
            if (includeReferences)
                return db.Photos.Where(x => x.Id == id).Include(p => p.Categories).FirstOrDefault();
            return db.Photos.FirstOrDefault(p => p.Id == id);
        }

        public static Photo GetPhotoByTitle(string title)
        {
            using PhotoDbContext db = new PhotoDbContext();
            return db.Photos.FirstOrDefault(p => p.Title == title);
        }

        public static List<Photo> GetPhotosByTitle(string title)
        {
            using PhotoDbContext db = new PhotoDbContext();
            return db.Photos.Where(p => p.Title == title).ToList();
        }

        public static List<Category> GetAllCategories()
        {
            using PhotoDbContext db = new PhotoDbContext();
            return db.Categories.ToList();
        }


        public static void InsertPhoto(Photo photo, List<string> selectedCategories)
        {

            using PhotoDbContext db = new PhotoDbContext();
            photo.Categories = new List<Category>();
            if (selectedCategories != null)
            {
                // Trasformiamo gli ID scelti in ingredienti da aggiungere tra i riferimenti in Pizza
                foreach (var category in selectedCategories)
                {
                    int id = int.Parse(category);
                    // NON usiamo un GetIngredientById() perché userebbe un db context diverso
                    // e ciò causerebbe errore in fase di salvataggio - usiamo lo stesso context all'interno della stessa operazione
                    var categoryFromDb = db.Categories.FirstOrDefault(x => x.Id == id);
                    if (categoryFromDb != null)
                    {
                        photo.Categories.Add(categoryFromDb);
                    }
                }
            }
            db.Photos.Add(photo);
            db.SaveChanges();
            
        }



        public static bool UpdatePhoto(int id, Photo photo, List<string> selectedCategories)
        {
            try
            {
                using PhotoDbContext db = new PhotoDbContext();
                var existingPhoto = db.Photos.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);
                if (existingPhoto == null)
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
                        var categoryFromDb = db.Categories.FirstOrDefault(x => x.Id == categoryId);
                        if (categoryFromDb != null)
                        {
                            existingPhoto.Categories.Add(categoryFromDb);
                        }
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log exception here (ex)
                return false;
            }
        }


        public static bool DeletePhoto(int id)
        {
            try
            {
                using PhotoDbContext db = new PhotoDbContext();
                var photoToDelete = db.Photos.FirstOrDefault(p => p.Id == id);
                if (photoToDelete == null)
                    return false;

                db.Photos.Remove(photoToDelete);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log exception here (ex)
                return false;
            }
        }

        public static void SeedPhotos()
        {
            if (PhotoManager.CountAllPhotos() == 0)
            {
                //crea 5 foto
                PhotoManager.InsertPhoto(new Photo { Title = "Sunset", Description = "Beautiful sunset over the hills", IsVisible = true}, new());
                PhotoManager.InsertPhoto(new Photo { Title = "Mountains", Description = "Snow-capped mountains", IsVisible = true}, new());
                PhotoManager.InsertPhoto(new Photo { Title = "Forest", Description = "Dense forest in the morning mist", IsVisible = true}, new());
                PhotoManager.InsertPhoto(new Photo { Title = "Ocean", Description = "Clear blue ocean", IsVisible = true}, new());
                PhotoManager.InsertPhoto(new Photo { Title = "Cityscape", Description = "City skyline at night", IsVisible = true}, new());
            }
        }

        public static void SeedCategories()
        {
            using PhotoDbContext db = new PhotoDbContext();

            // Controlla se ci sono già delle categorie nel database
            if (!db.Categories.Any())
            {
                // Crea alcune categorie predefinite
                var categories = new List<Category>
                {
                    new Category { Title = "Nature" },
                    new Category { Title = "Urban" },
                    new Category { Title = "Abstract" },
                    new Category { Title = "Portrait" },
                    new Category { Title = "Landscape" }
                };

                // Aggiunge le categorie al database
                db.Categories.AddRange(categories);
                db.SaveChanges();
            }
        }
    }
}
