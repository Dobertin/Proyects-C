using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jocsan.Models
{
    public class Abono
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAbono { get; set; }
        public string? Descripcion { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaAbono { get; set; }
        public decimal ValorAbono { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }
    }

}
