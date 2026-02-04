using Api.Application.DTOs;
using Api.Domain.Entities;
using MongoDB.Driver;

namespace Api.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User> Create(CreateUserDto dto);

        Task<List<User>> FindAll(
            FilterDefinition<User>? match = null,
            ProjectionDefinition<User>? project = null,
            SortDefinition<User>? sort = null
        );

        Task<User?> FindOne(
            FilterDefinition<User> match,
            ProjectionDefinition<User>? project = null
        );
    }
}
