namespace Sonatto.Models
{
    public class Carrinho
    {
        public int IdCarrinho { get; set; }
        public int IdUsuario { get; set; }
        public DateOnly DataCriaco { get; set; }
        public bool Estado { get; set; }
        public decimal ValorTotal {  get; set; }

    }
}
