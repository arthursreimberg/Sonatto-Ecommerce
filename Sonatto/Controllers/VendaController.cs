using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;

namespace Sonatto.Controllers
{
    public class VendaController : Controller
    {
        private readonly IVendaAplicacao _vendaAplicacao;
        private readonly ICarrinhoAplicacao _carrinhoAplicacao;

        public VendaController(IVendaAplicacao vendaAplicacao, ICarrinhoAplicacao carrinhoAplicacao)
        {
            _vendaAplicacao = vendaAplicacao;
            _carrinhoAplicacao = carrinhoAplicacao;
        }

        // GET: /Venda/Buscar?idUsuario=5
        [HttpGet]
        public async Task<IActionResult> Buscar(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                    return Json(new { sucesso = false, mensagem = "IdUsuario inválido." });

                var venda = await _vendaAplicacao.BuscarVendas(idUsuario);

                return Json(new { sucesso = true, dados = venda });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = $"Erro ao buscar venda: {ex.Message}" });
            }
        }

        // POST: /Venda/Gerar
        [HttpPost]
        public async Task<IActionResult> Gerar(int idUsuario, string tipoPag, int idCarrinho)
        {
            try
            {
                if (idUsuario <= 0 || idCarrinho <= 0)
                    return Json(new { sucesso = false, mensagem = "Dados inválidos para gerar venda." });

                await _vendaAplicacao.GerarVenda(idUsuario, tipoPag, idCarrinho);

                // após gerar a venda, desativa o carrinho para que não seja mais retornado como disponível
                await _carrinhoAplicacao.DesativarCarrinho(idCarrinho);

                return Json(new { sucesso = true, mensagem = "Venda gerada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = $"Erro ao gerar venda: {ex.Message}" });
            }
        }
    }
}
