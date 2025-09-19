using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sonatto.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public required string nomeCliente { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 dígitos numéricos.")]
        public required int CPF {  get; set; }

        [Required(ErrorMessage = "O Email é obrigatório.")]
        public required string emailCliente { get; set; }

        [Required(ErrorMessage = "A senha é obrigatório.")]
        public required string senha {  get; set; }

        [Required(ErrorMessage = "O Telefone é obrigatório.")]
        public required string telefone { get; set; }

    }
}
