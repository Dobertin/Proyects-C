using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Facturacion.Models
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Usar 'Id' en lugar de 'ID' para evitar conflictos

        [BsonElement("NombreUsuario")]
        public string NombreUsuario { get; set; }

        [BsonElement("Contraseña")]
        public string Contraseña { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Roles")]
        public List<string> Roles { get; set; }
    }
}
