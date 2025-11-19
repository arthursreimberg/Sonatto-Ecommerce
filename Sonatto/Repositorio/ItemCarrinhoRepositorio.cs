using Dapper;
using MySql.Data.MySqlClient;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;
using System.Data;

namespace Sonatto.Repositorio
{
    public class ItemCarrinhoRepositorio : IItemCarrinhoRepositorio
    {
        private readonly string _connectionString;

        public ItemCarrinhoRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Usa a procedure sp_AdministrarCarrinho e retorna o IdCarrinho resultante
        public async Task<int> AdiconarItemCarrinho(int idUsuario, int idProduto, int qtd)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            using var tran = await conn.BeginTransactionAsync();
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("vIdUsuario", idUsuario, DbType.Int32);
                parametros.Add("vIdProduto", idProduto, DbType.Int32);
                parametros.Add("vQtd", qtd, DbType.Int32);

                // Chama a procedure que você criou
                await conn.ExecuteAsync("sp_AdministrarCarrinho", parametros, commandType: CommandType.StoredProcedure, transaction: tran);

                // Recupera o carrinho ativo do usuário (criado/atualizado pela procedure)
                var sqlGetCarrinho = @"SELECT IdCarrinho FROM tbCarrinho WHERE IdUsuario = @IdUsuario AND Estado = 1 ORDER BY IdCarrinho DESC LIMIT 1;";
                var idCarrinho = await conn.QueryFirstOrDefaultAsync<int?>(sqlGetCarrinho, new { IdUsuario = idUsuario }, transaction: tran);

                await tran.CommitAsync();

                return idCarrinho ?? 0;
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        // Usa a procedure sp_AlterarQuantidadeItem
        public async Task AlterarQuantidade(int idCarrinho, int idProduto, int qtd)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            using var tran = await conn.BeginTransactionAsync();
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("vIdCarrinho", idCarrinho, DbType.Int32);
                parametros.Add("vIdProduto", idProduto, DbType.Int32);
                parametros.Add("vNovaQtd", qtd, DbType.Int32);

                await conn.ExecuteAsync("sp_AlterarQuantidadeItem", parametros, commandType: CommandType.StoredProcedure, transaction: tran);

                // A procedure já atualiza SubTotal e ValorTotal; só comitar
                await tran.CommitAsync();
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        // Usa a procedure sp_RemoverItemCarrinho
        public async Task ApagarItemCarrinho(int idItem)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            using var tran = await conn.BeginTransactionAsync();
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("vIdItemCarrinho", idItem, DbType.Int32);

                await conn.ExecuteAsync("sp_RemoverItemCarrinho", parametros, commandType: CommandType.StoredProcedure, transaction: tran);

                // A procedure já ajusta o total do carrinho; comitar
                await tran.CommitAsync();
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        // Busca itens do carrinho com nome e primeira imagem do produto para exibição
        public async Task<IEnumerable<ItemCarrinho>> BuscarItensCarrinho(int idCarrinho)
        {
            using var conn = new MySqlConnection(_connectionString);

            var sql = @"
                SELECT ic.IdItemCarrinho, ic.IdCarrinho, ic.IdProduto, ic.QtdItemCar, ic.PrecoUnidadeCar,
                       ic.SubTotal,
                       p.NomeProduto as ProdutoNome,
                       (
                           SELECT ip.UrlImagem FROM tbImagens ip WHERE ip.IdProduto = p.IdProduto LIMIT 1
                       ) as ProdutoImagemUrl
                FROM tbItemCarrinho ic
                LEFT JOIN tbProduto p ON ic.IdProduto = p.IdProduto
                WHERE ic.IdCarrinho = @IdCarrinho;";

            var items = (await conn.QueryAsync<ItemCarrinho>(sql, new { IdCarrinho = idCarrinho })).ToList();

            // Normaliza URL da imagem
            foreach (var it in items)
            {
                if (!string.IsNullOrEmpty(it.ProdutoImagemUrl) && !it.ProdutoImagemUrl.StartsWith("/") && !it.ProdutoImagemUrl.StartsWith("http"))
                {
                    it.ProdutoImagemUrl = "/" + it.ProdutoImagemUrl.TrimStart('~', '/');
                }
            }

            return items;
        }
    }
}