using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace net_il_mio_fotoalbum.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Photo> PhotoId { get; set; }
    }

}
