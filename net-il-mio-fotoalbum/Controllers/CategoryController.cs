using Microsoft.AspNetCore.Mvc;

namespace net_il_mio_fotoalbum.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
