namespace Sonatto.Models
{
    public class HistoricoAcao
    {
        public int IdHistorico { get; set; }
        public int IdUsuario { get; set; }
        public string? Acao { get; set; }
        public DateTime DataAcao { get; set;}
    }
}
