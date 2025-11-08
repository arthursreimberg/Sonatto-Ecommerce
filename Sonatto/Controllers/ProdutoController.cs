using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Catalogo(int pagina = 1)
        {
            int produtosPorPagina = 9; // 3x3

            // Pega todos os produtos do banco
            var todosProdutos = await _produtoAplicacao.GetTodosAsync();

            // Paginação
            var produtos = todosProdutos
                           .Skip((pagina - 1) * produtosPorPagina)
                           .Take(produtosPorPagina)
                           .ToList();

            // Informações para a view
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)todosProdutos.Count() / produtosPorPagina);

            return View(produtos);
        }





        // EXIBIR DETALHES DE UM PRODUTO
        public async Task<IActionResult> Produto(int id)
        {
            var produto = await _produtoAplicacao.GetPorIdAsync(id);

            if (produto == null)
                return NotFound();

            return View(produto);
        }


        // GET: FORMULÁRIO DE CADASTRO
        public IActionResult Cadastrar()
        {
            return View();
        }


        // POST: CADASTRA PRODUTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(Produto produto, int qtdEstoque)
        {
            // Aqui você pega o ID do usuário logado (Session)
            int? idUsu = HttpContext.Session.GetInt32("UserId");

            if (idUsu == null)
                return RedirectToAction("Login", "Login");

            if (!ModelState.IsValid)
                return View(produto);

            await _produtoAplicacao.AdicionarProduto(produto, qtdEstoque, idUsu.Value);

            return RedirectToAction(nameof(Index));
        }


        // GET: EDITAR PRODUTO
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


   


        // ADICIONAR IMAGEM AO PRODUTO
        [HttpPost]
        public async Task<IActionResult> AdicionarImagem(int idProduto, string urlImagem)
        {
            await _produtoAplicacao.AdicionarImagens(idProduto, urlImagem);
            return RedirectToAction(nameof(Produto), new { id = idProduto });
        }
    }
}
