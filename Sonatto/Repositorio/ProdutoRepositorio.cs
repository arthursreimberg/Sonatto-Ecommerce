using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Sonatto.Models;

namespace Sonatto.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly string _connectionString; // string de conexão
        //contém as informações para se conectar ao banco

        public ProdutoRepositorio(string connectionString) //classe responsável por acessar os dados de produtos
        {
            _connectionString = connectionString;
        }

        public async Task AdicionarProdutoComImagens(Produto produto)
        {
            var imagensJson = JsonConvert.SerializeObject(produto.Imagens.Select(i => i.UrlImagem));

            using var conn = new MySqlConnection("Server=localhost;Database=SeuBanco;Uid=root;Pwd=sua_senha;");
            await conn.ExecuteAsync(
                "spAdicionarProdutoComImagens",
                new
                {
                    pNomeProduto = produto.NomeProduto,
                    pPreco = produto.Preco,
                    pImagemURL = produto.ImagemUrl,
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
            //var sql = "SELECT IdProduto, NomeProduto, Preco, ImageUrl, Marca FROM tbProduto WHERE IdProduto = @Id";
            //return await connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
            //Retorna o primeiro resultado se encontrar, retorna null se não encontrar (por isso o retorno é Produto?).

            var sql = @"
               SELECT 
                   p.IdProduto, p.NomeProduto, p.Preco, p.ImagemUrl, p.Marca,
                   i.IdImagem, i.IdProduto AS ImgIdProduto, i.UrlImagem
               FROM tbProduto p
               LEFT JOIN tbImagensProduto i ON p.IdProduto = i.IdProduto
               WHERE p.IdProduto = @Id;";

            Produto? produto = null;

            var result = await connection.QueryAsync<Produto, ImagemProduto, Produto>(
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

            //return produto;
            return await connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Produto>> GetTodosAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            //var sql = "SELECT IdProduto, NomeProduto, Preco, ImageUrl, Marca FROM tbProduto";            
            //return await connection.QueryAsync<Produto>(sql);
            //Usa Dapper.QueryAsync<Produto>() e executa o SQL e mapeia automaticamente os resultados para objetos da classe Produto.

            var sql = @"
               SELECT 
                   p.IdProduto, p.NomeProduto, p.Preco, p.ImagemUrl, p.Marca,
                   i.IdImagem, i.IdProduto AS ImgIdProduto, i.UrlImagem
               FROM tbProduto p
               LEFT JOIN tbImagensProduto i ON p.IdProduto = i.IdProduto;";

            var produtoDict = new Dictionary<int, Produto>();

            var result = await connection.QueryAsync<Produto, ImagemProduto, Produto>(
                sql,
                (produto, imagem) =>
                {
                    if (!produtoDict.TryGetValue(produto.IdProduto, out var prod))
                    {
                        prod = produto;
                        prod.Imagens = new List<ImagemProduto>();
                        produtoDict.Add(prod.IdProduto, prod);
                    }

                    if (imagem != null && imagem.IdImagem != 0)
                        prod.Imagens.Add(imagem);

                    return prod;
                },
                splitOn: "IdImagem"
            );

            return produtoDict.Values;
            //return await connection.QueryAsync<Produto>(sql);

        }

        /*






public async Task AdicionarProdutoComImagens(Produto produto)
{
   var imagensJson = JsonConvert.SerializeObject(produto.Imagens.Select(i => i.UrlImagem));

   using var conn = new MySqlConnection("Server=localhost;Database=SeuBanco;Uid=root;Pwd=sua_senha;");
   await conn.ExecuteAsync(
       "spAdicionarProdutoComImagens",
       new
       {
           pNomeProduto = produto.NomeProduto,
           pPreco = produto.Preco,
           pImagemURL = produto.ImagemUrl,
           pMarca = produto.Marca,
           pJsonImagens = imagensJson
       },
       commandType: System.Data.CommandType.StoredProcedure
   );
}
*/
    }
}
