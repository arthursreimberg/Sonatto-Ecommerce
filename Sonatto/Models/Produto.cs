using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sonatto.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100)]
        public required string NomeProduto { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public required decimal Preco { get; set; }

        [StringLength(255)]
        [Display(Name = "URL da Imagem")]
        [Required(ErrorMessage = "A imagem do produto é obrigatória.")]
        public required string ImagemUrl { get; set; }

        [Required(ErrorMessage = "A marca do produto é obrigatória.")]
        [StringLength(100)]
        public required string Marca { get; set; }

        public ICollection<ImagemProduto> Imagens { get; set; } = new List<ImagemProduto>();
    }
}
