using Org.BouncyCastle.Bcpg.OpenPgp;
using Sonatto.Models;
namespace Sonatto.Repositorio.Interfaces
{
    public interface IVendaRepositorio
    {
        Task<IEnumerable<Venda?>> BuscarVendas(int idUsuario);
        Task GerarVenda(int idUsuario, string tipoPag, int idCarrinho);
    }
}
