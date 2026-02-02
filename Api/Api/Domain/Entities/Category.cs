using Api.Infrastructure.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.Domain.Entities
{
    [BsonCollection("category")]
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("deletedAt")]
        [BsonIgnoreIfNull]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
