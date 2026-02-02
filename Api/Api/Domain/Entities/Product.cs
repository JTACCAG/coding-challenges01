using Api.Infrastructure.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.Domain.Entities
{
    [BsonCollection("product")]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("price")]
        public decimal Price{ get; set; }

        [BsonElement("stockQuantity")]
        public int StockQuantity { get; set; }

        [BsonElement("categoryIds")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string[] CategoryIds { get; set; } = [];

        public Category[] Categories { get; set; } = null!;

        [BsonElement("deletedAt")]
        [BsonIgnoreIfNull]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
