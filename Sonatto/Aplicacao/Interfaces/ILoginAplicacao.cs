using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface ILoginAplicacao
    {
        Task<Usuario> ValidarUsuario(string email, string senha);
    }
}
