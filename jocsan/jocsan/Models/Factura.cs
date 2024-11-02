using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jocsan.Models
{
    public class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFactura { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Porcentaje { get; set; }
        public int Galones { get; set; }
        public int Hielo { get; set; }
        public decimal SubTotalProd { get; set; }
        [Column("g-h")]
        public decimal GH { get; set; }
        public decimal SubTotalGH { get; set; }
        public decimal Terceros { get; set; }
        public decimal Peladores { get; set; }
        public decimal SubTotal { get; set; }
        [Column("25")]
        public decimal Valor25 { get; set; }
        [Column("13")]
        public decimal Valor13 { get; set; }
        public decimal Abono { get; set; }
        public decimal TotalVenta { get; set; }
        public int Estado { get; set; } = 1;
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }
        public ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }

}
