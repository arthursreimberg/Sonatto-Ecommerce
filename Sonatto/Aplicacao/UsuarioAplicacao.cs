using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Aplicacao
{
    public class UsuarioAplicacao : IUsuarioAplicacao
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }


        public async Task AdicionarNivelAsync(int id, int nivelId)
        {
            await _usuarioRepositorio.AdicionarNivel(id, nivelId);
        }

        public Task AlterarUsuarioAsync(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CadastrarUsuarioAsync(Usuario usuario)
        {
            var usuarioCadastro = await _usuarioRepositorio.ObterPorEmail(usuario.Email);
            if(usuarioCadastro != null)
            {
                throw new Exception("EMAIL_JA_CADASTRADO");
            }
            var idGerado = await _usuarioRepositorio.CadastrarUsuario(usuario);

            usuario.IdUsuario = idGerado; // atualiza o objeto

            return idGerado;
        }

    }
}
