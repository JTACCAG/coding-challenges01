namespace Api.Infrastructure.Mongo
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BsonCollectionAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}