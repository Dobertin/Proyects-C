using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jocsan.Models
{
    public class MovimientosCredito
    {
        [Key]
        public int IdMovimiento { get; set; }
        public int IdCredito { get; set; }
        public int IdAbono { get; set; }
        public decimal cantidad { get; set; }
        public DateTime fechaMovimiento { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        [ForeignKey(nameof(IdCredito))]
        public Credito credito { get; set; }
        [ForeignKey(nameof(IdAbono))]
        public Abono abono { get; set; }

    }
}
