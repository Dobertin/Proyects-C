using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Facturacion.Models
{
    public class FacturaDTO
    {
        public int ID { get; set; }

        [Required]
        public int ClienteID { get; set; }
        public DateTime FechaEmision { get; set; }

        [Required]
        public List<FacturaProductoDTO> Productos { get; set; } = new List<FacturaProductoDTO>();

        public List<FacturaImpuestoDTO> Impuestos { get; set; } = new List<FacturaImpuestoDTO>();

        public List<FacturaDescuentoDTO> Descuentos { get; set; } = new List<FacturaDescuentoDTO>();

        [Required]
        public int MetodoPagoID { get; set; }

        [Required]
        public string Estado { get; set; }
    }

    public class FacturaProductoDTO
    {
        public int ProductoID { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal Precio { get; set; }
    }

    public class FacturaImpuestoDTO
    {
        public int TipoImpuestoID { get; set; }

        [Required]
        public decimal Monto { get; set; }
    }

    public class FacturaDescuentoDTO
    {
        public int PromocionID { get; set; }

        [Required]
        public decimal Monto { get; set; }
    }
}
