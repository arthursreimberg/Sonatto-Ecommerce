using Sonatto.Models;
namespace Sonatto.Repositorio.Interfaces;
public interface IItemVendaRepositorio
{
    Task<IEnumerable<ItemVenda?>> BuscarItensVenda(int idVenda);
}
