using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Facturacion.Models
{
    public class TipoPago
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string _id { get; set; }

        [BsonElement("ID")]
        public int ID { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
