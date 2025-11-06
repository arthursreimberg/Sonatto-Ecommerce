using Microsoft.AspNetCore.Mvc;
using Sonatto.Repositorio;
using System.Linq;

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
        public async Task<IActionResult> Catalogo(int pagina = 1)
        {
            int itensPorPagina = 9;

            var todosOsProdutos = await _produtoRepo.GetTodosAsync();

            var produtos = todosOsProdutos.ToList();

            int totalItens = produtos.Count;
            int totalPaginas = (int)Math.Ceiling(totalItens / (double)itensPorPagina);

            pagina = Math.Max(1, Math.Min(pagina, totalPaginas == 0 ? 1 : totalPaginas));

            var produtosPaginados = produtos
                .Skip((pagina - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToList();

            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(produtosPaginados);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var produto = await _produtoRepo.GetPorIdAsync(id);
            if (produto == null) return NotFound();
            return View(produto);
        }
    }
}
