using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Facturacion.Models
{
    public class Rol
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Usar 'Id' en lugar de 'ID' para evitar conflictos

        [BsonElement("Nombre")]
        public string Nombre { get; set; }
    }
}
