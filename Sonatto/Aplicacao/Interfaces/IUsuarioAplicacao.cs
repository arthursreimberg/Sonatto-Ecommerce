using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface IUsuarioAplicacao
    {
        Task<int> CadastrarUsuarioAsync(Usuario usuario);
        Task AdicionarNivelAsync(int id, int nivelId);
        Task AlterarUsuarioAsync(Usuario usuario);
    }
}
