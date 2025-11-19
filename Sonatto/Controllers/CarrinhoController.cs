using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;

namespace Sonatto.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ICarrinhoAplicacao _carrinhoAplicacao;
        private readonly IItemCarrinhoAplicacao _itemCarrinhoAplicacao;

        public CarrinhoController(ICarrinhoAplicacao carrinhoAplicacao, IItemCarrinhoAplicacao itemCarrinhoAplicacao)
        {
            _carrinhoAplicacao = carrinhoAplicacao;
            _itemCarrinhoAplicacao = itemCarrinhoAplicacao;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? idUsuario = HttpContext.Session.GetInt32("UserId");
            if (idUsuario == null)
                return RedirectToAction("Index", "Login");

            var carrinho = await _carrinhoAplicacao.BuscarCarrinho(idUsuario.Value);
            if (carrinho == null)
                return View("Carrinho", new CarrinhoViewModel { Carrinho = null, Items = Enumerable.Empty<ItemCarrinho>() });

            var itens = (await _itemCarrinhoAplicacao.BuscarItensCarrinho(carrinho.IdCarrinho)).ToList();

            // normaliza urls para a view server-side
            foreach (var it in itens)
            {
                if (!string.IsNullOrWhiteSpace(it.ProdutoImagemUrl) && !it.ProdutoImagemUrl.StartsWith("/") && !it.ProdutoImagemUrl.StartsWith("http"))
                {
                    it.ProdutoImagemUrl = Url.Content(it.ProdutoImagemUrl);
                }
            }

            ViewBag.UsuarioId = idUsuario.Value;
            var vm = new CarrinhoViewModel { Carrinho = carrinho, Items = itens };
            return View("Carrinho", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(int? idUsuario)
        {
            var effectiveUser = idUsuario;
            if (!effectiveUser.HasValue || effectiveUser.Value <= 0)
                effectiveUser = HttpContext.Session.GetInt32("UserId");

            if (!effectiveUser.HasValue || effectiveUser.Value <= 0)
                return Json(new { sucesso = false, mensagem = "Usuário não autenticado." });

            try
            {
                var carrinho = await _carrinhoAplicacao.BuscarCarrinho(effectiveUser.Value);
                if (carrinho == null)
                    return Json(new { sucesso = false, mensagem = "Nenhum carrinho encontrado." });

                var itens = (await _itemCarrinhoAplicacao.BuscarItensCarrinho(carrinho.IdCarrinho)).ToList();

                var itensDto = itens.Select(i => new
                {
                    idItemCarrinho = i.IdItemCarrinho,
                    idCarrinho = i.IdCarrinho,
                    idProduto = i.IdProduto,
                    qtdItemCar = i.QtdItemCar,
                    precoUnidadeCar = i.PrecoUnidadeCar,
                    subTotal = i.SubTotal,
                    produtoNome = i.ProdutoNome,
                    produtoImagemUrl = string.IsNullOrWhiteSpace(i.ProdutoImagemUrl) ? Url.Content("~/imgs/Produto/Bateria1.jpg") : (i.ProdutoImagemUrl.StartsWith("/") || i.ProdutoImagemUrl.StartsWith("http") ? i.ProdutoImagemUrl : Url.Content(i.ProdutoImagemUrl))
                }).ToList();

                return Json(new { sucesso = true, dados = new { carrinho, itens = itensDto } });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = $"Erro ao buscar carrinho: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Desativar(int idCarrinho)
        {
            try
            {
                if (idCarrinho <= 0)
                    return Json(new { sucesso = false, mensagem = "IdCarrinho inválido." });

                await _carrinhoAplicacao.DesativarCarrinho(idCarrinho);

                return Json(new { sucesso = true, mensagem = "Carrinho desativado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = $"Erro ao desativar carrinho: {ex.Message}" });
            }
        }
    }
}
