using System;

namespace Sonatto.Models
{
    public class AcaoUsuario
    {
        public int IdAcao { get; set; }
        public int IdUsuario { get; set; }
        public string NomeAcao { get; set; } = null!;
        public int? IdNivel { get; set; }
        public DateTime DataAcao { get; set; }

        // opcional: nome do nível (quando fizer JOIN)
        public string? NivelNome { get; set; }
    }
}
