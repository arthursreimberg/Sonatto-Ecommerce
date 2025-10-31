﻿using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Aplicacao
{
    public class LoginAplicacao : ILoginAplicacao
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public LoginAplicacao( IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        public async Task<Usuario> ValidarUsuario(string email, string senha)
        {
            var usuarioLogin = await _usuarioRepositorio.ObterPorEmailSenha(email, senha);

            if (usuarioLogin == null)
            {
                throw new Exception("Usuário ou senha inválido e/ou usuário não encontrado.");
            }

            return usuarioLogin;
        }
    }
}
