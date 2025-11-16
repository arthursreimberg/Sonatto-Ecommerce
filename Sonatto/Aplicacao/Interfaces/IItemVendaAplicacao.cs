using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface IItemVendaAplicacao
    {
        Task<IEnumerable<ItemVenda?>> BuscarItensVenda(int idVenda);
    }
}
