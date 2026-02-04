using Api.Application.DTOs;
using Api.Domain.Entities;

namespace Api.Application.Services
{
    public interface IUserService
    {
        Task<User> Created(CreateUserDto dto);

        Task<User?> FindOne(string id);

        Task<User?> GetByEmail(string email);

        Task ValidateByEmail(string email);
    }
}
