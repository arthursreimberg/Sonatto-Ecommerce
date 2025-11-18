using Dapper;
using MySql.Data.MySqlClient;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Repositorio
{
    public class ItemVendaRepositorio : IItemVendaRepositorio
    {
        private readonly string _connectionString;

        public ItemVendaRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IEnumerable<ItemVenda?>> BuscarItensVenda(int idVenda)
        {
            using var conn = new MySqlConnection(_connectionString);

            var sql = @"SELECT * FROM tbItemVenda WHERE IdVenda = @IdVenda;";

            return await conn.QueryAsync<ItemVenda>(sql, new { IdVenda = idVenda});
        }
    }
}
