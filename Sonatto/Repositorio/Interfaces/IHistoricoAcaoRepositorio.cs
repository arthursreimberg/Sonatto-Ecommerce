using Sonatto.Models;

namespace Sonatto.Repositorio.Interfaces
{
    public interface IHistoricoAcaoRepositorio
    {
        Task<IEnumerable<HistoricoAcao>> BuscarHistoricoAcao();
        Task<IEnumerable<HistoricoAcao>> BuscarHistoricoAcaoFunc(int idUsuario);
    }
}
