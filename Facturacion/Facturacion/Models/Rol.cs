using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Facturacion.Models
{
    public class Rol
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("Nombre")]
        public string Nombre { get; set; }
    }
}
