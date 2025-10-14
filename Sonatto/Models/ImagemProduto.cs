using System.ComponentModel.DataAnnotations;

namespace Sonatto.Models
{
    public class ImagemProduto
    {
        public int IdImagem { get; set; }

        [Required]
        public int IdProduto { get; set; } // FK

        [Required]
        [StringLength(255)]
        public string UrlImagem { get; set; } = string.Empty;
        [Required]
        public Produto Produto { get; set; } = null!;

    }
}
