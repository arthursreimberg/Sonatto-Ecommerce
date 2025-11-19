using Dapper;
using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Repositorio
{
    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {
        private readonly string _connectionString;

        public CarrinhoRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<Carrinho?> BuscarCarrinho(int idUsuario)
        {
                using var conn = new MySqlConnection(_connectionString);

                // Retorna apenas o carrinho ativo/ disponível (Estado = 1)
                var sql = @"SELECT * FROM tbCarrinho WHERE IdUsuario = @IdUsuario AND Estado = 1 ORDER BY IdCarrinho DESC LIMIT 1;";

                return await conn.QueryFirstOrDefaultAsync<Carrinho>(sql, new { IdUsuario = idUsuario});
        }

        public async Task DesativarCarrinho(int idCarrinho)
        {
            using var conn = new MySqlConnection(_connectionString);

            var sql = @"UPDATE tbCarrinho SET Estado = 0 WHERE IdCarrinho = @IdCarrinho;";

            await conn.ExecuteAsync(sql, new { IdCarrinho = idCarrinho });
        }
    }
}
