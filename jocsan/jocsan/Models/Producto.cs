using System.ComponentModel.DataAnnotations;

namespace jocsan.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        public string? Codigo { get; set; }
        public string? NombreLocal { get; set; }
        public decimal Precio { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        public ICollection<DetalleFactura> DetalleFacturas { get; set; }
    }

}
