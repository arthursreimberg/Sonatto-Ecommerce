using Dapper;
using MySql.Data.MySqlClient;
using Sonatto.Models;
using System.Data;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string _connectionString; // string de conexão

        public UsuarioRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> CadastrarUsuario(Usuario usuario)
        {
            using var conn = new MySqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("vEmail", usuario.Email);
            parametros.Add("vNome", usuario.Nome);
            parametros.Add("vSenha", usuario.Senha);
            parametros.Add("vCPF", usuario.CPF);
            parametros.Add("vEndereco", usuario.Endereco);
            parametros.Add("vTelefone", usuario.Telefone);

            parametros.Add("vIdCli", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("sp_CadastroUsu", parametros, commandType: CommandType.StoredProcedure);

            int idUsuario = parametros.Get<int>("vIdCli");

            return idUsuario;
        }

        public async Task<Usuario> ObterPorEmailSenha(string email, string senha)
        {
            using var conn = new MySqlConnection(_connectionString);

            var sql = "SELECT * FROM tbUsuario WHERE Email = @Email AND Senha = @Senha";

            return await conn.QueryFirstOrDefaultAsync<Usuario>(sql, new
            {
                Email = email,
                Senha = senha
            });
        }
    }
}
