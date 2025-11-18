using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface IHistoricoAcaoAplicacao
    {
        Task<IEnumerable<HistoricoAcao>> BuscarHistoricoAcao();
        Task<IEnumerable<HistoricoAcao>> BuscarHistoricoAcaoFunc(int idUsuario);
    }
}
