using Api.Application.DTOs;
using Api.Application.Enums;
using Api.Domain.Entities;
using Api.Infrastructure.Mongo;
using MongoDB.Driver;

namespace Api.Application.Repositories
{
    public class UserRepository(MongoService mongo) : IUserRepository
    {
        private readonly IMongoCollection<User> _model = mongo.GetCollection<User>();

        public async Task<User> Create(CreateUserDto dto)
        {
            var created = new User
            {
                Fullname = dto.Fullname,
                Email= dto.Email,
                Password = dto.Password,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = RoleEnum.Regular,
            };
            await _model.InsertOneAsync(created);
            return created;
        }

        public async Task<List<User>> FindAll(
            FilterDefinition<User>? match = null,
            ProjectionDefinition<User>? project = null,
            SortDefinition<User>? sort = null
         )
        {
            var query = match is not null ? _model.Find(match) : _model.Find(_ => true);
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
            var query = _model.Find(match);
            if (project != null)
            {
                query = query.Project<User>(project);
            }
            return await query.FirstOrDefaultAsync();
        }
    }
}
