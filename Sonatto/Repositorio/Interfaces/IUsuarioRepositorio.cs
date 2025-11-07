using Sonatto.Models;
using System.ComponentModel.DataAnnotations;

namespace Sonatto.Repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario?> ObterPorEmailSenha(string email, string senha);
        Task<int> CadastrarUsuario(Usuario usuario);
        Task AlterarUsuario(Usuario usuario);
        Task<Usuario?> ObterPorEmail(string email);
        Task AdicionarNivel(int idUsu, int nivelId);
    }
}
