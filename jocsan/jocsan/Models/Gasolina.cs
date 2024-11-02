using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jocsan.Models
{
    public class Gasolina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGasolina { get; set; }
        public string? Comentario { get; set; }
        public int IdCliente { get; set; }
        public decimal PrecioGalonPagado { get; set; }
        public int CantGalonPagado { get; set; }
        public decimal PrecioGalonCargado { get; set; }
        public int CantGalonCargado { get; set; }
        public decimal TotalGalonCargado { get; set; }
        public decimal TotalGalonPagado { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }
    }
}
