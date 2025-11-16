namespace Sonatto.Models
{
    public class ItemVenda
    {
        public int IdItemVenda { get; set; }
        public int IdVenda { get; set; }
        public int IdProduto { get; set; }
        public decimal PrecoUni { get; set; }
        public decimal Qtd { get; set; }
    }
}
