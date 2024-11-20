using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace jocsan.Models
{
    public class Vuelto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVuelto { get; set; }
        public string? Comentario { get; set; }
        public int IdCliente { get; set; }
        public int Estado { get; set; } = 1;
        public int TipoVuelto { get; set; }
        public DateTime FechaVuelto { get; set; }
        public decimal Monto { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }
    }
}
