using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Facturacion.Models
{
    public class Counter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonElement("CollectionName")]
        public string CollectionName { get; set; }

        [BsonElement("SequenceValue")]
        public int SequenceValue { get; set; }
    }
}
