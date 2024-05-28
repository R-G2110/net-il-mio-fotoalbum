using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoWebApiController : ControllerBase
    {
        [HttpGet("{name?}")]
        public IActionResult GetAllPhotos(string? title = "") 
        {
            if (string.IsNullOrWhiteSpace(title))
                return Ok(PhotoManager.GetAllVisiblePhotos());
            return Ok(PhotoManager.GetPhotosByTitle(title));
        }

        [HttpGet]
        public IActionResult GetPhotoById(int id) 
        {
            var photo = PhotoManager.GetPhoto(id);
            if (photo == null)
                return NotFound("Photo not found!");
            return Ok(photo);
        }

        [HttpGet("{name}")]
        public IActionResult GetPhotoByTitle(string title) 
        {
            var photo = PhotoManager.GetPhotoByTitle(title);
            if (photo == null)
                return NotFound("Photo not found!");
            return Ok(photo);
        }

        
    }
}