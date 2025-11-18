using Sonatto.Models;
using System.ComponentModel.DataAnnotations;

namespace Sonatto.Repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario?> ObterPorId(int idUsuario);

        Task<Usuario?> ObterPorEmailSenha(string email, string senha);
        Task<int> CadastrarUsuario(Usuario usuario);
        Task AlterarUsuario(Usuario usuario);
        Task<Usuario?> ObterPorEmail(string email);
        Task AdicionarNivel(int idUsu, int nivelId);

        // Retorna lista de nomes dos níveis atribuídos ao usuário (ex: "Nivel 1", "Administrador")
        Task<IEnumerable<string>> GetNiveisPorUsuario(int idUsuario);

        // Histórico de ações do usuário
        Task<IEnumerable<AcaoUsuario>> GetAcoesPorUsuario(int idUsuario, int limite = 50);
    }
}
