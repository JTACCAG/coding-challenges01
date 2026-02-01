using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Infrastructure.Mongo
{
    public class MongoService
    {
        private readonly IMongoDatabase _database;

        public MongoService(IOptions<MongoSettings> options)
        {
            var settings = options.Value;

            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var collectionName = typeof(T)
                .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault() is BsonCollectionAttribute attr
                    ? attr.Name
                    : typeof(T).Name;

            return _database.GetCollection<T>(collectionName);
        }
    }
}
