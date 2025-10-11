using Microsoft.AspNetCore.Mvc;

namespace Sonatto.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Produto()
        {
            return View();
        }
        public IActionResult Produtos()
        {
            return View();
        }
    }
}
