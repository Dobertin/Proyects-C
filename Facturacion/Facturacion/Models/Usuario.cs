using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Facturacion.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault] // Ignora el campo si su valor es el predeterminado
        private string _id { get; set; }

        [BsonElement("ID")]
        public int ID { get; set; }

        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
