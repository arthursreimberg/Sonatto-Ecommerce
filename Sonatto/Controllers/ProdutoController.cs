using Microsoft.AspNetCore.Mvc;
using Sonatto.Repositorio;

namespace Sonatto.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepo;

        public ProdutoController(IProdutoRepositorio produtoRepo)
        {
            _produtoRepo = produtoRepo;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepo.GetTodosAsync();
            return View(produtos);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var produto = await _produtoRepo.GetPorIdAsync(id);
            if (produto == null) return NotFound();
            return View(produto);
        }
    }
}
