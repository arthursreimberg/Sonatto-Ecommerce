using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;

namespace Sonatto.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginAplicacao _loginAplicacao;

        public LoginController(ILoginAplicacao loginAplicacao)
        {
            _loginAplicacao = loginAplicacao;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string senha)
        {
            // Verifica campos vazios
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                TempData["Mensagem"] = "Preencha todos os campos.";
                TempData["TipoMensagem"] = "warning";
                return RedirectToAction("Index");
            }

            var usuario = await _loginAplicacao.ValidarUsuario(email, senha);

            if (usuario == null)
            {
                TempData["Mensagem"] = "Usuário ou senha inválidos.";
                TempData["TipoMensagem"] = "danger";
                return RedirectToAction("Index");
            }

            // Login OK → cria sessão
            HttpContext.Session.SetInt32("UserId", usuario.IdUsuario);
            HttpContext.Session.SetString("UserNome", usuario.Nome);

            return RedirectToAction("Index", "Home");
        }

        //Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
