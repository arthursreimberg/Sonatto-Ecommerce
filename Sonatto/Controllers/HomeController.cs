using Microsoft.AspNetCore.Mvc;
using Sonatto.Models;
using Sonatto.Repositorio;
using System.Diagnostics;

namespace Sonatto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProdutoRepositorio _produtoRepositorio;

        //usado para registrar logs da aplicação(mensagens de erro, avisos, etc).
        public HomeController(ILogger<HomeController> logger, ProdutoRepositorio produtoRepositorio)
        {
            _logger = logger;
            _produtoRepositorio = produtoRepositorio; //classe que acessa os produtos
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepositorio.GetTodosAsync(); //Chama do repositório, que retorna (de forma assíncrona) todos os produtos da loja.
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
