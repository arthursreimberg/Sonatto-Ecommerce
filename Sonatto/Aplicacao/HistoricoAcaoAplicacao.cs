using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Aplicacao
{
    public class HistoricoAcaoAplicacao : IHistoricoAcaoAplicacao
    {
        private readonly IHistoricoAcaoRepositorio _historicoAcaoRepositorio;

        public HistoricoAcaoAplicacao(IHistoricoAcaoRepositorio historicoAcaoRepositorio)
        {
            _historicoAcaoRepositorio = historicoAcaoRepositorio;
        }
        public async Task<IEnumerable<HistoricoAcao>> BuscarHistoricoAcao()
        {
            return await _historicoAcaoRepositorio.BuscarHistoricoAcao();
        }

        public async Task<IEnumerable<HistoricoAcao>> BuscarHistoricoAcaoFunc(int idUsuario)
        {
            return await _historicoAcaoRepositorio.BuscarHistoricoAcaoFunc(idUsuario);
        }
    }
}
