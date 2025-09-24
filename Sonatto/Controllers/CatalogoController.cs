using Microsoft.AspNetCore.Mvc;

namespace Sonatto.Controllers
{
    public class CatalogoController : Controller
    {
        public IActionResult Catalogo()
        {
            return View();
        }
    }
}
