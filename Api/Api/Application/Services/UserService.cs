using Api.Application.Repositories;
using Api.Domain.Entities;
using Api.Infrastructure.Mongo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Api.Application.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<User?> GetByEmail(string email)
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Email, email)
            );
            var projection = Builders<User>.Projection
                .Include(u => u.Email)
                .Include(u => u.Fullname)
                .Include(u => u.Password)
                .Include(u => u.Role);
            return await _userRepository.FindOne(filter, projection);
        }
    }
}
