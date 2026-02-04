using Api.Application.DTOs;
using Api.Application.Exceptions;
using Api.Application.Repositories;
using Api.Domain.Entities;
using Api.Infrastructure.Mongo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Created(CreateUserDto dto)
        {
            return await _userRepository.Create(dto);
        }

        public async Task ValidateByEmail(string email)
        {
            var user = await GetByEmail(email);
            if (user is not null)
                throw new NotFoundException("El email proporcionado ya se encuentra registrado en nuestro sistema");
        }

        public async Task<List<User>> FindAll(SearchUserDto dto)
        {
            var users = await _userRepository.FindAll();
            return users;
        }

        public async Task<User?> FindOne(string id)
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.Id, id)
            );
            var user = await _userRepository.FindOne(filter);
            if (user == null)
                return null;
            return user;
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

        public Task<User> Create(CreateUserDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
