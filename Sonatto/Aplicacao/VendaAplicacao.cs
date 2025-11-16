using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Aplicacao
{
    public class VendaAplicacao : IVendaAplicacao
    {
        private readonly IVendaRepositorio _vendaRepositorio;

        public VendaAplicacao(IVendaRepositorio vendaRepositorio)
        {
            _vendaRepositorio = vendaRepositorio;
        }
        public async Task<Venda?> BuscarVendas(int idUsuario)
        {
            return await _vendaRepositorio.BuscarVendas(idUsuario);
        }

        public async Task GerarVenda(int idUsuario, string tipoPag, int idCarrinho)
        {
            await _vendaRepositorio.GerarVenda(idUsuario, tipoPag, idCarrinho);
        }
    }
}
