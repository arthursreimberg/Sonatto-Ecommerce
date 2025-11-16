using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Aplicacao
{
    public class ItemVendaAplicacao : IItemVendaAplicacao
    {
        private readonly IItemVendaRepositorio _itemVendaRepositorio;

        public ItemVendaAplicacao(IItemVendaRepositorio itemVendaRepositorio)
        {
            _itemVendaRepositorio = itemVendaRepositorio;
        }
        public async Task<IEnumerable<ItemVenda?>> BuscarItensVenda(int idVenda)
        {
            return await _itemVendaRepositorio.BuscarItensVenda(idVenda);
        }
    }
}
