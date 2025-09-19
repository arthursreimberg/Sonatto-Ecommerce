namespace Sonatto.Models
{
    public class Estoque
    {
        public int idEstoque { get; set; }
        public int idProduto { get; set; }
        public int quantidade { get; set; }
        public bool disponibilidade { get; set; }
    }
}
