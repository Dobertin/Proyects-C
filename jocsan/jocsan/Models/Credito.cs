using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jocsan.Models
{
    public class Credito
    {
        [Key]
        public int IdCredito { get; set; }
        public string? Descripcion { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaCredito { get; set; }
        public decimal ValorCredito { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }
    }

}
