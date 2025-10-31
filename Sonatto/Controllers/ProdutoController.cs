using Microsoft.AspNetCore.Mvc;
using Sonatto.Repositorio;

namespace Sonatto.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepo;

        public class GlobalNavProd
        {
            public string IdProd { get; set; }
        }
        public ProdutoController(IProdutoRepositorio produtoRepo)
        {
            _produtoRepo = produtoRepo;
        }

        public async Task<IActionResult> Index()
        {
            var IdDoProduto = "";
            var GlobalVar = "";

            var produtos = await _produtoRepo.GetTodosAsync();
            //{
            //    IdDoProduto = produtos(IdDoProduto).Id;
            //}

            return View(produtos);
        }
        public async Task<IActionResult> Catalogo()
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
