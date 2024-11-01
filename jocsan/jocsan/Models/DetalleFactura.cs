﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jocsan.Models
{
    public class DetalleFactura
    {
        [Key]
        public int IdDetalleFactura { get; set; }
        public int IdProducto { get; set; }
        public int IdFactura { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Cantidad { get; set; }
        public decimal SubTotalParcial { get; set; }
        public decimal TotalParcial { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioModifica { get; set; }
        public DateTime? FechaModifica { get; set; }

        [ForeignKey(nameof(IdFactura))]
        public Factura Factura { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public Producto Producto { get; set; }
    }

}
