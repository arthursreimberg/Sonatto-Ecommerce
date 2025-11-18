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

        public async Task<int> CadastrarUsuarioAsync(Usuario usuario)
        {
            return await _usuarioRepositorio.CadastrarUsuario(usuario);
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _usuarioRepositorio.ObterPorEmail(email);
        }

        public async Task<Usuario?> ObterPorEmailSenhaAsync(string email, string senha)
        {
            return await _usuarioRepositorio.ObterPorEmailSenha(email, senha);
        }

        public async Task<Usuario?> ObterPorIdAsync(int idUsuario)
        {
            return await _usuarioRepositorio.ObterPorId(idUsuario);
        }

        public async Task AlterarUsuarioAsync(Usuario usuario)
        {
            await _usuarioRepositorio.AlterarUsuario(usuario);
        }

        public async Task AdicionarNivelAsync(int idUsu, int nivelId)
        {
            await _usuarioRepositorio.AdicionarNivel(idUsu, nivelId);
        }

        // Novo: repassa para o repositório
        public async Task<IEnumerable<string>> GetNiveisPorUsuarioAsync(int idUsuario)
        {
            return await _usuarioRepositorio.GetNiveisPorUsuario(idUsuario);
        }

        public async Task<IEnumerable<AcaoUsuario>> GetAcoesPorUsuarioAsync(int idUsuario, int limite = 50)
        {
            return await _usuarioRepositorio.GetAcoesPorUsuario(idUsuario, limite);
        }
    }
}
