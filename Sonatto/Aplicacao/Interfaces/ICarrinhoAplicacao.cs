using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface ICarrinhoAplicacao
    {
        Task<Carrinho?> BuscarCarrinho(int idUsuario);
        Task DesativarCarrinho(int idCarrinho);
    }
}
