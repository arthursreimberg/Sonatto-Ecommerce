using Sonatto.Models;

namespace Sonatto.Repositorio.Interfaces
{
    public interface ICarrinhoRepositorio
    {
        Task<Carrinho?> BuscarCarrinho(int idUsuario);
        Task DesativarCarrinho(int idCarrinho);
    }
}
