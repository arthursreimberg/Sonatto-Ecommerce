using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Aplicacao
{
    public class CarrinhoAplicacao : ICarrinhoAplicacao
    {
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;

        public CarrinhoAplicacao(ICarrinhoRepositorio carrinhoRepositorio)
        {
            _carrinhoRepositorio = carrinhoRepositorio;
        }
        public async Task<Carrinho?> BuscarCarrinho(int idUsuario)
        {
            return await _carrinhoRepositorio.BuscarCarrinho(idUsuario);
        }

        public async Task DesativarCarrinho(int idCarrinho)
        {
            await _carrinhoRepositorio.DesativarCarrinho(idCarrinho);
        }
    }
}
