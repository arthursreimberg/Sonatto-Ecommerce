using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Repositorio;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginAplicacao _loginAplicacao;

        public LoginController(ILoginAplicacao loginAplicacao)
        {
            _loginAplicacao = loginAplicacao;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string senha)
        {

            var usuario = await _loginAplicacao.ValidarUsuario(email, senha);

            // Se usuário ou senha inválidos
            if (usuario == null)
            {
                TempData["Mensagem"] = "Usuário ou senha inválidos.";
                TempData["TipoMensagem"] = "danger"; // Vermelho
                return RedirectToAction("Index");
            }

            // Login válido → cria sessão
            HttpContext.Session.SetInt32("UserId", usuario.IdUsuario);
            HttpContext.Session.SetString("UserNome", usuario.Nome);

            return RedirectToAction("Index", "Home");
        }


        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
