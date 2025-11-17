using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;

namespace Sonatto.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ICarrinhoAplicacao _carrinhoAplicacao;

        public CarrinhoController(ICarrinhoAplicacao carrinhoAplicacao)
        {
            _carrinhoAplicacao = carrinhoAplicacao;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(int idUsuario)
        {
            try
            {
                var carrinho = await _carrinhoAplicacao.BuscarCarrinho(idUsuario);

                if (carrinho == null)
                    return Json(new { sucesso = false, mensagem = "Nenhum carrinho encontrado." });

                return Json(new { sucesso = true, dados = carrinho });
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
