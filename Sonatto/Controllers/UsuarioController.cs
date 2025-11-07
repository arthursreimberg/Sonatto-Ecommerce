using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;

namespace Sonatto.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IUsuarioAplicacao _usuarioAplicacao;

        public UsuarioController(IUsuarioAplicacao usuarioAplicacao)
        {
            _usuarioAplicacao = usuarioAplicacao;
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(Usuario usuario)
        {
            var idGerado = await _usuarioAplicacao.CadastrarUsuarioAsync(usuario);

            usuario.IdUsuario = idGerado;

            HttpContext.Session.SetInt32("UserId", usuario.IdUsuario);
            HttpContext.Session.SetString("UserNome", usuario.Nome);

            return RedirectToAction("Index", "Home");
        }


    }
}
