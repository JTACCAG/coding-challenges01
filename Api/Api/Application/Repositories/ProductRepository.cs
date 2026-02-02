using Api.Application.DTOs;
using Api.Domain.Entities;
using Api.Infrastructure.Mongo;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MongoDB.Driver;

namespace Api.Application.Repositories
{
    public class ProductRepository(MongoService mongo)
    {
        private readonly IMongoCollection<Product> _model = mongo.GetCollection<Product>();

        public async Task<Product> Create(CreateProductDto dto)
        {
            var created = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryIds = dto.CategoryIds,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _model.InsertOneAsync(created);
            return created;
        }

        public async Task<List<Product>> FindAll(
            FilterDefinition<Product>? match = null,
            ProjectionDefinition<Product>? project = null,
            SortDefinition<Product>? sort = null
         )
        {
            var query = match is not null ? _model.Find(match) : _model.Find(_ => true);
            if (project is not null)
                query = query.Project<Product>(project);
            if (sort is not null)
                query = query.Sort(sort);
            return await query.ToListAsync();
        }

        public async Task<Product?> FindOne(
            FilterDefinition<Product> match,
            ProjectionDefinition<Product>? project = null
        )
        {
            var query = _model.Find(match);
            if (project != null)
            {
                query = query.Project<Product>(project);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product> Upsert(string id, Product model)
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(u => u.Id, id)
            );
            var options = new FindOneAndUpdateOptions<Product>
            {
                ReturnDocument = ReturnDocument.After
            };
            var update = Builders<Product>.Update
                .Set(p => p.Name, model.Name)
                .Set(p => p.Description, model.Description)
                .Set(p => p.Price, model.Price)
                .Set(p => p.StockQuantity, model.StockQuantity)
                .Set(p => p.CategoryIds, model.CategoryIds)
                .Set(p => p.UpdatedAt, DateTime.Now);
            var response = await _model.FindOneAndUpdateAsync(filter, update, options);
            return response;
        }

        public async Task<Product> Delete(string id)
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(u => u.Id, id)
            );
            var response = await _model.FindOneAndDeleteAsync(filter);
            return response;
        }
    }
}
