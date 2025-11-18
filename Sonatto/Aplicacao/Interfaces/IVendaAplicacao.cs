using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface IVendaAplicacao
    {
        Task<IEnumerable<Venda?>> BuscarVendas(int idUsuario);
        Task GerarVenda(int idUsuario, string tipoPag, int idCarrinho);
    }
}
