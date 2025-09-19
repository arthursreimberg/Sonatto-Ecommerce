using System.ComponentModel.DataAnnotations;

namespace Sonatto.Models
{
    public class Compra
    {
        public int idCompra { get; set; }
        public DateOnly dataCompra { get; set; }
        public double valorTotal { get; set; }

        [Required(ErrorMessage = "O tipo de pagamento é obrigatório.")]
        public required string tipoPagamento { get; set; }

    }
}
