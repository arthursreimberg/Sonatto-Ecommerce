namespace Sonatto.Models
{
    public class ItemCarrinho
    {
        public int idProduto { get; set; }
        public int quantidade { get; set; }
        public double precoUnitario { get; set; }

        public double Total => quantidade * precoUnitario;

    }
}
