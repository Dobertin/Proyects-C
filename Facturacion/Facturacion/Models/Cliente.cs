using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Facturacion.Models
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault] // Ignora el campo si su valor es el predeterminado
        private string _id { get; set; }

        [BsonElement("ID")]
        public int ID { get; set; }

        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
