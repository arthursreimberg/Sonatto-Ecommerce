namespace Sonatto.Models
{
    public class Funcionario
    {
        public int idFuncionario { get; set; }
        public required string nome { get; set; }
        public required string cargo { get; set; }
        public required string email { get; set; }
        public required string senha { get; set; }
    }
}
