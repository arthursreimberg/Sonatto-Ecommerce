using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;

namespace Sonatto.Controllers
{
    public class ItemVendaController : Controller
    {
        private readonly IItemVendaAplicacao _itemVendaAplicacao;

        public ItemVendaController(IItemVendaAplicacao itemVendaAplicacao)
        {
            _itemVendaAplicacao = itemVendaAplicacao;
        }

        // GET: /ItemVenda/Buscar?idVenda=10
        [HttpGet]
        public async Task<IActionResult> Buscar(int idVenda)
        {
            try
            {
                if (idVenda <= 0)
                    return Json(new { sucesso = false, mensagem = "IdVenda inválido." });

                var itens = await _itemVendaAplicacao.BuscarItensVenda(idVenda);

                return Json(new { sucesso = true, dados = itens });
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = $"Erro ao buscar itens da venda: {ex.Message}" });
            }
        }
    }
}
