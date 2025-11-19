using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface IItemCarrinhoAplicacao
    {
        Task AlterarQuantidade(int idCarrinho, int idProduto, int qtd);
        Task ApagarItemCarrinho(int idItem);
        Task<int> AdiconarItemCarrinho(int idUsuario, int idProduto, int qtd);
        Task<IEnumerable<ItemCarrinho>> BuscarItensCarrinho(int idCarrinho);
    }
}
