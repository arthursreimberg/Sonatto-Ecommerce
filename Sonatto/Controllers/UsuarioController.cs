using Microsoft.AspNetCore.Mvc;
using Sonatto.Aplicacao.Interfaces;
using Sonatto.Models;
using System.Text.RegularExpressions;

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
        public async Task<IActionResult> VerificarAcesso()
        {
            int? idUsuario = HttpContext.Session.GetInt32("UserId");
            if (idUsuario == null)
            {
                return Json(new { authenticated = false });
            }

            var niveis = (await _usuarioAplicacao.GetNiveisPorUsuarioAsync(idUsuario.Value)).ToList();

            if (niveis.Any(n => n != null && (n.Equals("Administrador", System.StringComparison.OrdinalIgnoreCase)
                                   || n.Equals("Admin", System.StringComparison.OrdinalIgnoreCase))))
            {
                return Json(new { authenticated = true, redirect = Url.Action("Administrador", "Usuario") });
            }

            if (niveis.Any())
            {
                return Json(new { authenticated = true, redirect = Url.Action("Funcionario", "Usuario") });
            }

            return Json(new { authenticated = true, redirect = Url.Action("Perfil", "Usuario") });
        }

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            int? idUsuario = HttpContext.Session.GetInt32("UserId");
            if (idUsuario == null)
                return RedirectToAction("Login", "Login");

            var usuario = await _usuarioAplicacao.ObterPorIdAsync(idUsuario.Value);
            ViewBag.Usuario = usuario;

            var vendas = await _vendaAplicacao.BuscarVendas(idUsuario.Value);
            ViewBag.Vendas = vendas;

            var itensPorVenda = new Dictionary<int, List<ItemVenda>>();
            foreach (var venda in vendas)
            {
                var itens = await _itemVendaAplicacao.BuscarItensVenda(venda.IdVenda);
                itensPorVenda[venda.IdVenda] = itens.ToList();
            }
            ViewBag.ItensPorVenda = itensPorVenda;

            var produtos = await _produtoAplicacao.GetTodosAsync();
            ViewBag.Produtos = produtos;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alterar(Usuario usuario)
        {
            int? idSess = HttpContext.Session.GetInt32("UserId");
            if (idSess == null || usuario == null || usuario.IdUsuario != idSess.Value)
            {
                return Forbid();
            }

            await _usuarioAplicacao.AlterarUsuarioAsync(usuario);

            HttpContext.Session.SetString("UserNome", usuario.Nome ?? string.Empty);

            return RedirectToAction("Perfil");
        }

        // Load recent actions and levels for the funcionario view
        public async Task<IActionResult> Funcionario()
        {
            int? idUsuario = HttpContext.Session.GetInt32("UserId");
            if (idUsuario == null)
                return RedirectToAction("Login", "Login");

            // carrega histórico de ações
            var acoes = (await _usuarioAplicacao.GetAcoesPorUsuarioAsync(idUsuario.Value, 50)).ToList();
            ViewBag.Acoes = acoes;

            // carrega níveis para controle de permissões na view (opcional)
            var niveis = (await _usuarioAplicacao.GetNiveisPorUsuarioAsync(idUsuario.Value)).ToList();
            ViewBag.Niveis = niveis;

            return View();
        }

        public IActionResult Administrador()
        {
            return View();
        }

        // Verifica se usuário tem permissão para uma ação específica e retorna URL para redirecionamento
        [HttpGet]
        public async Task<IActionResult> AcessarAcao(string acao)
        {
            int? idUsuario = HttpContext.Session.GetInt32("UserId");
            if (idUsuario == null)
                return Json(new { allowed = false, redirect = Url.Action("Login", "Login") });

            var niveis = (await _usuarioAplicacao.GetNiveisPorUsuarioAsync(idUsuario.Value)).ToList();
            var niveisNorm = niveis
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.Trim())
                .ToList();

            // detect admin
            bool isAdmin = niveisNorm.Any(n => n.Equals("Administrador", System.StringComparison.OrdinalIgnoreCase)
                                               || n.Equals("Admin", System.StringComparison.OrdinalIgnoreCase));

            // map action to required level
            int nivelRequerido = acao?.ToUpperInvariant() switch
            {
                "ADICIONAR" => 1,
                "EDITAR" => 2,
                "DELETAR" => 3,
                _ => 999
            };

            // helper: try parse number from nivel string (handles 'Nivel 1', 'Nivel l', etc.)
            int? ParseNivel(string s)
            {
                if (string.IsNullOrWhiteSpace(s)) return null;
                s = s.Trim();

                // try digits first
                var m = Regex.Match(s, @"(\d+)");
                if (m.Success && int.TryParse(m.Value, out var num)) return num;

                // common typo: letter 'l' or 'I' used instead of '1' (e.g., 'Nivel l')
                if (Regex.IsMatch(s, @"nivel\s*[lLiI]$", RegexOptions.IgnoreCase)) return 1;

                return null;
            }

            var niveisNum = niveisNorm.Select(ParseNivel).Where(x => x.HasValue).Select(x => x!.Value).ToList();

            bool possui = false;
            if (isAdmin) possui = true;
            else if (nivelRequerido != 999) possui = niveisNum.Contains(nivelRequerido);

            // determine redirect URL for the action
            string redirectUrl = acao?.ToUpperInvariant() switch
            {
                "ADICIONAR" => Url.Action("Adicionar", "Produto"),
                "EDITAR" => Url.Action("Catalogo", "Produto"),
                "DELETAR" => Url.Action("Catalogo", "Produto"),
                _ => Url.Action("Perfil", "Usuario")
            } ?? Url.Action("Perfil", "Usuario");

            if (possui)
            {
                return Json(new { allowed = true, redirect = redirectUrl });
            }

            return Json(new { allowed = false, redirect = Url.Action("Perfil", "Usuario") });
        }
    }
}
