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
        [HttpGet("{title?}")]
        public IActionResult GetAllPhotos(string? title = "")
        {
            if (string.IsNullOrWhiteSpace(title))
                return Ok(PhotoManager.GetAllVisiblePhotos());
            return Ok(PhotoManager.GetAllVisiblePhotosByTitle(title));
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] Message messageToSend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            // Salva il messaggio nel database
            MessageManager.SaveMessage(messageToSend);

            return Ok("Message sent successfully!");
        }

    }
}
