using Api.Application.Enums;
using Api.Infrastructure.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace Api.Domain.Entities
{
    public class LowercaseEnumSerializer<T> : SerializerBase<T> where T : struct, Enum
    {
        public override T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            return Enum.Parse<T>(value, true); // ignore case
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            context.Writer.WriteString(value.ToString().ToLower());
        }
    }

    [BsonIgnoreExtraElements]
    [BsonCollection("user")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("password")]
        public string Password { get; set; } = null!;

        [BsonElement("fullname")]
        public string Fullname { get; set; } = null!;

        [BsonElement("deletedAt")]
        [BsonIgnoreIfNull]
        public DateTime? DeletedAt { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("role")]
        [BsonSerializer(typeof(LowercaseEnumSerializer<RoleEnum>))]
        public RoleEnum Role { get; set; } = RoleEnum.Regular;
    }
}
