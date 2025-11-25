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
        private readonly IHistoricoAcaoAplicacao _historicoAcaoAplicacao;
        public UsuarioController(
            IUsuarioAplicacao usuarioAplicacao,
            IProdutoAplicacao produtoAplicacao,
            IVendaAplicacao vendaAplicacao,
            IItemVendaAplicacao itemVendaAplicacao,
            IHistoricoAcaoAplicacao historicoAplicacao
        )
        {
            _usuarioAplicacao = usuarioAplicacao;
            _produtoAplicacao = produtoAplicacao;
            _vendaAplicacao = vendaAplicacao;
            _historicoAcaoAplicacao = historicoAplicacao;
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

            //verifica se o nível do usuario é igual a Administrador
            if (niveis.Any(n => n != null && (n.Equals("Administrador", System.StringComparison.OrdinalIgnoreCase)
                                   || n.Equals("Admin", System.StringComparison.OrdinalIgnoreCase))))
            {
                return Json(new { authenticated = true, redirect = Url.Action("Administrador", "Usuario") });
            }
            //verifica se o usuário possui algum nível de acesso
            if (niveis.Any())
            {
                return Json(new { authenticated = true, redirect = Url.Action("Funcionario", "Usuario") });
            }

            //caso as verificações forem falsa, ent vai pro perfil
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
            var acoes = await _historicoAcaoAplicacao.BuscarHistoricoAcaoFunc(idUsuario.Value);
            ViewBag.Acoes = acoes;


            return View();
        }

        public async Task<IActionResult> Administrador()
        {
            var historico = await _historicoAcaoAplicacao.BuscarHistoricoAcao();
            ViewBag.Historico = historico;

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

            // verifica se é admin
            bool isAdmin = niveisNorm.Any(n => n.Equals("Administrador", System.StringComparison.OrdinalIgnoreCase)
                                               || n.Equals("Admin", System.StringComparison.OrdinalIgnoreCase));

            // mapa das ações
            int nivelRequerido = acao?.ToUpperInvariant() switch
            {
                "ADICIONAR" => 1,
                "EDITAR" => 2,
                "DELETAR" => 3,
                _ => 999
            };

            var niveisNum = new List<int>();
            foreach (var s in niveisNorm)
            {
                if (string.IsNullOrWhiteSpace(s)) continue;
                var texto = s.Trim();

                // caso específico: 'Nivel l' (letra L) => 1
                if (texto.Equals("nivel l", System.StringComparison.OrdinalIgnoreCase))
                {
                    niveisNum.Add(1);
                    continue;
                }

                // formato esperado: 'Nivel 2', 'Nivel 3' -> pega a parte após 'Nivel'
                if (texto.StartsWith("nivel", System.StringComparison.OrdinalIgnoreCase))
                {
                    var parte = texto.Substring(5).Trim(); // remove 'nivel'
                    if (int.TryParse(parte, out var n))
                    {
                        niveisNum.Add(n);
                        continue;
                    }
                }

                // se porventura já for apenas '1','2'...
                if (int.TryParse(texto, out var v))
                {
                    niveisNum.Add(v);
                }
            }

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
