using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface IProdutoAplicacao
    {
        Task<IEnumerable<Produto>> GetTodosAsync();
        Task<Produto?> GetPorIdAsync(int id);
        Task AdicionarImagens(int idProduto, string url);
        Task AdicionarProduto(Produto produto, int qtdEstoque, int idUsu);
        Task Alterar_e_DeletarProduto(Produto produto, int qtdEstoque, string acaoAlterar, int idUsu);
    }
}
