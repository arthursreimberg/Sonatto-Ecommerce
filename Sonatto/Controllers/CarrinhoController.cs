using Microsoft.AspNetCore.Mvc;

namespace Sonatto.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
