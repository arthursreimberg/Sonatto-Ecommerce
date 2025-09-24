namespace Sonatto.Models
{
    public class Produto
    {
        public required int IdProduto { get; set; }
        public required string NomeProduto { get; set; }
        public required decimal Preco {  get; set; }
        
        public required string ImagemUrl { get; set; }
    }
}
