using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;

namespace Sonatto.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IProdutoAplicacao _produtoAplicacao;
        private readonly IVendaAplicacao _vendaAplicacao;
        private readonly IItemVendaAplicacao _itemVendaAplicacao;

        public UsuarioController(
            IUsuarioAplicacao usuarioAplicacao,
            IProdutoAplicacao produtoAplicacao,
            IVendaAplicacao vendaAplicacao,
            IItemVendaAplicacao itemVendaAplicacao
        )
        {
            _usuarioAplicacao = usuarioAplicacao;
            _produtoAplicacao = produtoAplicacao;
            _vendaAplicacao = vendaAplicacao;
            _itemVendaAplicacao = itemVendaAplicacao;
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

        public IActionResult VerificarLogin()
        {
            bool usuarioLogado = HttpContext.Session.GetInt32("UserId") != null;
            return Json(usuarioLogado);
        }

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            int? idUsuario = HttpContext.Session.GetInt32("UserId");

            if (idUsuario == null)
                return RedirectToAction("Login", "Login");

            var usuario = await _usuarioAplicacao.ObterPorIdAsync(idUsuario.Value);

            var venda = await _vendaAplicacao.BuscarVendas(idUsuario.Value);

            IEnumerable<ItemVenda> itens = new List<ItemVenda>();

            if (venda != null)
                itens = await _itemVendaAplicacao.BuscarItensVenda(venda.IdVenda);

            ViewBag.Usuario = usuario;
            ViewBag.Venda = venda;
            ViewBag.ItensVenda = itens;

            var produtos = await _produtoAplicacao.GetTodosAsync();

            return View(produtos);
        }

        public IActionResult Funcionario()
        {
            return View();
        }

        public IActionResult Administrador()
        {
            return View();
        }
    }
}
