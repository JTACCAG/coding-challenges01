using Api.Domain.Entities;
using Api.Infrastructure.Mongo;
using MongoDB.Driver;

namespace Api.Application.Repositories
{
    public class UserRepository(MongoService mongo)
    {
        private readonly IMongoCollection<User> _model = mongo.GetCollection<User>();

        public async Task<List<User>> FindAll(
            FilterDefinition<User> match,
            ProjectionDefinition<User>? project = null,
            SortDefinition<User>? sort = null
         )
        {
            var query = _model.Find(match);
            if (project is not null)
                query = query.Project<User>(project);
            if (sort is not null)
                query = query.Sort(sort);
            return await query.ToListAsync();
        }

        public async Task<User?> FindOne(
            FilterDefinition<User> match,
            ProjectionDefinition<User>? project = null
        )
        {
            Console.WriteLine("FindOne");
            var user = await _model.Find(Builders<User>.Filter.Empty).FirstOrDefaultAsync();
            Console.WriteLine(user?.Id);
            var query = _model.Find(match);
            if (project != null)
            {
                Console.WriteLine("project");
                query = query.Project<User>(project);
            }
            return await query.FirstOrDefaultAsync();
        }
    }
}
