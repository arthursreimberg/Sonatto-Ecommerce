using Sonatto.Models;

namespace Sonatto.Repositorio.Interfaces
{
    public interface IItemCarrinhoRepositorio
    {
        Task AlterarQuantidade(int idCarrinho, int idProduto, int qtd);
        Task ApagarItemCarrinho(int idItem);
        Task AdiconarItemCarrinho(int idUsuario, int idProduto, int qtd);
        Task <IEnumerable<ItemCarrinho>> BuscarItensCarrinho(int idCarrinho);

    }
}
