using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;
using System.Diagnostics;

namespace Sonatto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProdutoAplicacao _produtoAplicacao;

        public HomeController(ILogger<HomeController> logger, IProdutoAplicacao produtoAplicacao)
        {
            _logger = logger;
            _produtoAplicacao = produtoAplicacao;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var produtos = await _produtoAplicacao.GetTodosAsync();
            return View(produtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
