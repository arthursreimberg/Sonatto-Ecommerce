using Dapper;
using MySql.Data.MySqlClient;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;
using System;
using System.Data;
using System.Security.Policy;

namespace Sonatto.Repositorio
{
    public class VendaRepositorio : IVendaRepositorio
    {
        private readonly string _connectionString;

        public VendaRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<Venda?> BuscarVendas(int idUsuario)
        {
            using var conn = new MySqlConnection(_connectionString);

            var sql = @"SELECT * FROM tbVenda WHERE IdUsuario = @IdUsuario;";

            return await conn.QueryFirstOrDefaultAsync<Venda>(sql, new { IdUsuario = idUsuario });
        }

        public async Task GerarVenda(int idUsuario, string tipoPag, int idCarrinho)
        {
            using var conn = new MySqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("vIdUsuario", idUsuario);
            parametros.Add("vTipoPag", tipoPag);
            parametros.Add("vIdCarrinho", idCarrinho);

            await conn.ExecuteAsync("sp_GerarVenda", parametros, commandType: CommandType.StoredProcedure);
        }
    }
}
