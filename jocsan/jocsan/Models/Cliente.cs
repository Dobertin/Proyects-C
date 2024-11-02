using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jocsan.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }
        public string? Nombre { get; set; }
        public string? Capitan { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Gasolina { get; set; }
        public bool NuevaEmbarcacion { get; set; }
        public string? Peon1 { get; set; }
        public string? Peon2 { get; set; }
        public int Estado { get; set; } = 1;
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }
        public ICollection<Abono> Abonos { get; set; }
        public ICollection<Credito> Creditos { get; set; }
    }

}
