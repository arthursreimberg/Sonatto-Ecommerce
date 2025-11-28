namespace Sonatto.Models
{
    public class UsuariosNiveisViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public string? ListaNiveis { get; set; }

        public List<string> ListaNiveisList =>
            (ListaNiveis ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();
    }
}
