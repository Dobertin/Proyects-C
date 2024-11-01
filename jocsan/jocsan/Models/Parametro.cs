using System.ComponentModel.DataAnnotations;

namespace jocsan.Models
{
    public class Parametro
    {
        [Key]
        public int IdParametro { get; set; }
        public int TipParametro { get; set; }
        public string? Descripcion { get; set; }
        public decimal ValorN { get; set; }
        public string? ValorT { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }
    }
}
