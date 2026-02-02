using Api.Application.DTOs;
using Api.Domain.Entities;
using Api.Infrastructure.Mongo;
using MongoDB.Driver;

namespace Api.Application.Repositories
{
    public class CategoryRepository(MongoService mongo)
    {
        private readonly IMongoCollection<Category> _model = mongo.GetCollection<Category>();

        public async Task<Category> Create(CreateCategoryDto dto)
        {
            var created = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _model.InsertOneAsync(created);
            return created;
        }

        public async Task<List<Category>> FindAll(
            FilterDefinition<Category> match,
            ProjectionDefinition<Category>? project = null,
            SortDefinition<Category>? sort = null
         )
        {
            var query = _model.Find(match);
            if (project is not null)
                query = query.Project<Category>(project);
            if (sort is not null)
                query = query.Sort(sort);
            return await query.ToListAsync();
        }

        public async Task<Category?> FindOne(
            FilterDefinition<Category> match,
            ProjectionDefinition<Category>? project = null
        )
        {
            var query = _model.Find(match);
            if (project != null)
            {
                query = query.Project<Category>(project);
            }
            return await query.FirstOrDefaultAsync();
        }
    }
}
