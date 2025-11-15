using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;

namespace Sonatto.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IProdutoAplicacao _produtoAplicacao;

        public UsuarioController(IUsuarioAplicacao usuarioAplicacao, IProdutoAplicacao produtoAplicacao)
        {
            _usuarioAplicacao = usuarioAplicacao;
            _produtoAplicacao = produtoAplicacao;
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

        // ✅ Verifica se existe um usuário logado
        public IActionResult VerificarLogin()
        {
            bool usuarioLogado = HttpContext.Session.GetInt32("UserId") != null;
            return Json(usuarioLogado);
        }

        [HttpGet]
        // ✅ Exibe o perfil ou redireciona para login
        public async Task<IActionResult> PerfilAsync()
        {
            var todosProdutos = await _produtoAplicacao.GetTodosAsync();

            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Login");

            return View(todosProdutos);
        }
    }
}
