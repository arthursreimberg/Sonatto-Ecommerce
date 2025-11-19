using Sonatto.Models;

namespace Sonatto.Repositorio.Interfaces
{
    public interface IItemCarrinhoRepositorio
    {
        Task<int> AdiconarItemCarrinho(int idUsuario, int idProduto, int qtd);
        Task AlterarQuantidade(int idCarrinho, int idProduto, int qtd);
        Task ApagarItemCarrinho(int idItem);
        Task<IEnumerable<ItemCarrinho>> BuscarItensCarrinho(int idCarrinho);
    }
}
