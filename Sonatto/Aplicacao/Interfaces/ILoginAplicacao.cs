using Sonatto.Models;

namespace Sonatto.Aplicacao.Interfaces
{
    public interface ILoginAplicacao
    {
        public Task<Usuario> ValidarUsuario(string email, string senha);
    }
}
