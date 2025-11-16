using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;

namespace Sonatto.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoAplicacao _produtoAplicacao;

        public ProdutoController(IProdutoAplicacao produtoAplicacao)
        {
            _produtoAplicacao = produtoAplicacao;
        }


        // Buscar produtos na barra de pesquisa e com a categoria pelo menu lateral
        [HttpGet]
        public async Task<IActionResult> BuscarProdutos(string search, string categoria)
        {
            var todosProdutos = await _produtoAplicacao.GetTodosAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                todosProdutos = todosProdutos
                    .Where(p => p.NomeProduto != null &&
                                p.NomeProduto.StartsWith(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                todosProdutos = todosProdutos
                    .Where(p => p.Categoria != null &&
                                p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return PartialView("_CardsProdutos", todosProdutos);
        }


        // Buscar todos os produtos para o catálogo
        public async Task<IActionResult> Catalogo(string search, string categoria, int pagina = 1)
        {
            int produtosPorPagina = 9;

            var todosProdutos = await _produtoAplicacao.GetTodosAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                todosProdutos = todosProdutos
                    .Where(p => p.NomeProduto != null &&
                                p.NomeProduto.StartsWith(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                todosProdutos = todosProdutos
                    .Where(p => p.Categoria != null &&
                                p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var produtos = todosProdutos
                .Skip((pagina - 1) * produtosPorPagina)
                .Take(produtosPorPagina)
                .ToList();

            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)todosProdutos.Count() / produtosPorPagina);

            ViewBag.Search = search;
            ViewBag.Categoria = categoria;

            return View(produtos);
        }


        public class ComboDeView
        {
            public Produto Produto { get; set; }
            public List<Produto> Produtos { get; set; }
        }
        
        public async Task<IActionResult> Produto(int id)
        {
            var produto = await _produtoAplicacao.GetPorIdAsync(id);
            var produtos = await _produtoAplicacao.GetTodosAsync(); 

            var visualizador = new ComboDeView
            {
                Produto = produto,
                Produtos = (List<Produto>)produtos
            };

            if (produto == null)
                return NotFound();

            return View(visualizador);
        }


        // Tela de Cadastro de Produto
        public IActionResult Adicionar()
        {
            return View();
        }


        // Método para cadastrar Produto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(Produto produto, int qtdEstoque, List<string> imagens)
        {
            int? idUsu = HttpContext.Session.GetInt32("UserId");
            if (idUsu == null)
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
                return View(produto);

            try
            {
                // 1️⃣ Adiciona o produto e obtém o ID
                int idProduto = await _produtoAplicacao.AdicionarProduto(produto, qtdEstoque, idUsu.Value);

                // 2️⃣ Adiciona as imagens
                if (imagens != null && imagens.Count > 0)
                {
                    foreach (var url in imagens)
                    {
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            await _produtoAplicacao.AdicionarImagens(idProduto, url);
                            await Task.Delay(150); // pequeno delay entre inserts
                        }
                    }
                }

                TempData["Sucesso"] = "Produto e imagens cadastrados com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro ao cadastrar produto: " + ex.Message;
                return View(produto);
            }
        }


        public async Task<IActionResult> Editar(int id)
        {
            var produto = await _produtoAplicacao.GetPorIdAsync(id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }


        // POST: EDITA PRODUTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Produto produto, int qtdEstoque)
        {
            int? idUsu = HttpContext.Session.GetInt32("UserId");

            await _produtoAplicacao.Alterar_e_DeletarProduto(produto, qtdEstoque, "ALTERAR", idUsu.Value);

            return RedirectToAction(nameof(Index));
        }


        // DELETAR PRODUTO
        public async Task<IActionResult> Deletar(int id)
        {
            var produto = await _produtoAplicacao.GetPorIdAsync(id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }

    }
}
