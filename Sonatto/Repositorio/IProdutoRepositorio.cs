using Sonatto.Models;

namespace Sonatto.Repositorio
{
    public interface IProdutoRepositorio
    {
        Task<IEnumerable<Produto>> GetTodosAsync();
        Task<Produto?> GetPorIdAsync(int id);
        Task<Produto?> AdicionarProdutoComImagens(int id);
        Task AdicionarProdutoComImagens(Produto produto);
    }
}
