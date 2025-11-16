using Dapper;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
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

        public async Task AdiconarItemCarrinho(int idUsuario, int idProduto, int qtd)
        {
            using var conn = new MySqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("vIdUsuario", idUsuario);
            parametros.Add("vIdProduto", idProduto);
            parametros.Add("vQtd", qtd);

            await conn.ExecuteAsync("sp_AdministrarCarrinho", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task AlterarQuantidade(int idCarrinho, int idProduto, int qtd)
        {
            using var conn = new MySqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("vIdCarrinho", idCarrinho);
            parametros.Add("vIdProduto", idProduto);
            parametros.Add("vNovaQtd", qtd);

            await conn.ExecuteAsync("sp_AlterarQuantidadeItem", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task ApagarItemCarrinho(int idItem)
        {
            using var conn = new MySqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("vIdItemCarrinho", idItem);

            await conn.ExecuteAsync("sp_RemoverItemCarrinho0", parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ItemCarrinho>> BuscarItensCarrinho(int idCarrinho)
        {
            using var conn = new MySqlConnection(_connectionString);

            var sql = @"SELECT * FROM tbItemCarrinho WHERE IdCarrinho = @IdCarrinho;";

            return await conn.QueryAsync<ItemCarrinho>(sql, new { IdCarrinho = idCarrinho });
        }
    }
}
