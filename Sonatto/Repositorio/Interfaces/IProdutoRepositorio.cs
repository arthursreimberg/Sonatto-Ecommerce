using Sonatto.Models;

namespace Sonatto.Repositorio.Interfaces
{
    public interface IProdutoRepositorio
    {
        Task<IEnumerable<Produto>> GetTodosAsync();
        Task<Produto?> GetPorIdAsync(int id);
        Task AdicionarImagens(int idProduto, string url);
        Task AdicionarProduto(Produto produto, int qtdEstoque, int idUsu);
        Task Alterar_e_DeletarProduto(Produto produto, int qtdEstoque, string acaoAlterar, int idUsu);

    }
}
