using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Facturacion.Models
{
    public class Promocion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        private string _id { get; set; }

        [BsonElement("ID")]
        public int ID { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; } // Ejemplo: "Descuento", "Oferta especial"
        public decimal Valor { get; set; } // Ejemplo: porcentaje de descuento
        public List<int> ProductoIDs { get; set; } = new List<int>();
        public List<string> Categorias { get; set; } = new List<string>();
    }
}
