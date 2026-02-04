using Api.Application.DTOs;
using Api.Domain.Entities;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Api.Application.Repositories
{
    public class ReportRepository(MongoService mongo)
    {
        private readonly IMongoCollection<Report> _model = mongo.GetCollection<Report>();

        public async Task<Report> Create(CreateReportDto dto)
        {
            var created = new Report
            {
                ProductId = dto.ProductId,
                UserId = dto.UserId,
                Reason = dto.Reason,
                Solved = dto.Solved,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _model.InsertOneAsync(created);
            return created;
        }

        public async Task<List<Report>> FindAll(
            FilterDefinition<Report> match,
            ProjectionDefinition<Report>? project = null,
            SortDefinition<Report>? sort = null
         )
        {
            var query = _model.Find(match);
            if (project is not null)
                query = query.Project<Report>(project);
            if (sort is not null)
                query = query.Sort(sort);
            return await query.ToListAsync();
        }

        public async Task<Report?> FindOne(
            FilterDefinition<Report> match,
            ProjectionDefinition<Report>? project = null
        )
        {
            var query = _model.Find(match);
            if (project != null)
            {
                query = query.Project<Report>(project);
            }
            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ReportWithProductDto>> GetReports()
        {
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { "solved", false }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "product" },
                    { "localField", "productId" },
                    { "foreignField", "_id" },
                    { "as", "product" }
                }),
                new BsonDocument("$unwind", "$product"),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "user" },
                    { "localField", "userId" },
                    { "foreignField", "_id" },
                    { "as", "user" }
                }),
                new BsonDocument("$unwind", "$user"),
            };
            var result = await _model.Aggregate<ReportWithProductDto>(pipeline).ToListAsync();
            return result;
        }
    }
}
