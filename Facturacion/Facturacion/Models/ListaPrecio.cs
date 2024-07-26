using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Facturacion.Models
{
    public class ListaPrecio
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        private string _id { get; set; }

        [BsonElement("ID")]
        public int ID { get; set; }

        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<PrecioProducto> Productos { get; set; } = new List<PrecioProducto>();
    }

    public class PrecioProducto
    {
        public int ProductoID { get; set; }
        public decimal Precio { get; set; }
    }
}
