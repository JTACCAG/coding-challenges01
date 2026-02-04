using Api.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.Domain.ValueObjects
{

    [BsonIgnoreExtraElements]
    public class ReportWithProductDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        // 👇 ESTA LÍNEA ES LA CLAVE
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } = null!;

        [BsonElement("product")]
        public Product Product { get; set; } = null!;

        [BsonElement("user")]
        public User User{ get; set; } = null!;

        [BsonElement("solved")]
        public bool Solved { get; set; }

        [BsonElement("reason")]
        public string Reason { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
