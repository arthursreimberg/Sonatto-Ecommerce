using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Sonatto.Models;
using Sonatto.Repositorio.Interfaces;

namespace Sonatto.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly string _connectionString; // string de conexão

        public ProdutoRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AdicionarProdutoComImagens(Produto produto)
        {
            var imagensJson = JsonConvert.SerializeObject(produto.Imagens.Select(i => i.UrlImagem));

            using var conn = new MySqlConnection(_connectionString); // ✅ agora usa a string do appsettings
            await conn.ExecuteAsync(
                "spAdicionarProdutoComImagens",
                new
                {
                    pNomeProduto = produto.NomeProduto,
                    pPreco = produto.Preco,
                    pMarca = produto.Marca,
                    pJsonImagens = imagensJson
                },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public Task<Produto?> AdicionarProdutoComImagens(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Produto?> GetPorIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = @"
               SELECT 
                   p.IdProduto, p.NomeProduto, p.Preco, p.ImagemUrl, p.Marca,
                   i.IdImagem, i.IdProduto AS ImgIdProduto, i.UrlImagem
               FROM tbProduto p
               LEFT JOIN tbImagens i ON p.IdProduto = i.IdProduto
               WHERE p.IdProduto = @Id;";

            Produto? produto = null;

            await connection.QueryAsync<Produto, ImagemProduto, Produto>(
                sql,
                (p, i) =>
                {
                    if (produto == null)
                    {
                        produto = p;
                        produto.Imagens = new List<ImagemProduto>();
                    }

                    if (i != null && i.IdImagem != 0)
                        produto.Imagens.Add(i);

                    return produto;
                },
                new { Id = id },
                splitOn: "IdImagem"
            );

            return produto; // ✅ agora retorna o produto com imagens
        }

        public async Task<IEnumerable<Produto>> GetTodosAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = @"
               SELECT 
<<<<<<< HEAD
                   p.IdProduto, p.NomeProduto, p.Preco, p.Marca,
=======
                   p.IdProduto, p.NomeProduto, p.Preco, p.Imagens, p.Marca,
>>>>>>> 669f2354cdd464340a1e7f0b885aeb496e6e5421
                   i.IdImagem, i.IdProduto AS ImgIdProduto, i.UrlImagem
               FROM tbProduto p
               LEFT JOIN tbImagens i ON p.IdProduto = i.IdProduto;";

            var produtos = new Dictionary<int, Produto>();

            await connection.QueryAsync<Produto, ImagemProduto, Produto>(
                sql,
                (produto, imagem) =>
                {
                    if (!produtos.TryGetValue(produto.IdProduto, out var prod))
                    {
                        prod = produto;
                        prod.Imagens = new List<ImagemProduto>();
                        produtos.Add(prod.IdProduto, prod);
                    }

                    if (imagem != null && imagem.IdImagem != 0)
                        prod.Imagens.Add(imagem);

                    return prod;
                },
                splitOn: "IdImagem"
            );

            return produtos.Values;
        }
    }
}
