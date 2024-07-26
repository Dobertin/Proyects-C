using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Facturacion.Models
{
    public class Factura
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string _id { get; set; }

        [BsonElement("ID")]
        public int ID { get; set; }

        public int ClienteID { get; set; }
        public DateTime FechaEmision { get; set; }
        public List<FacturaProducto> Productos { get; set; } = new List<FacturaProducto>();
        public decimal Total { get; set; }
        public List<FacturaImpuesto> Impuestos { get; set; } = new List<FacturaImpuesto>();
        public List<FacturaDescuento> Descuentos { get; set; } = new List<FacturaDescuento>();
        public int MetodoPagoID { get; set; }
        public string Estado { get; set; }
    }

    public class FacturaProducto
    {
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }

    public class FacturaImpuesto
    {
        public int TipoImpuestoID { get; set; }
        public decimal Monto { get; set; }
    }

    public class FacturaDescuento
    {
        public int PromocionID { get; set; }
        public decimal Monto { get; set; }
    }
}
