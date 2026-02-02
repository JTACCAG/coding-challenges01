using Api.Application.DTOs;
using Api.Application.Exceptions;
using Api.Application.Repositories;
using Api.Domain.Entities;
using MongoDB.Driver;

namespace Api.Application.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _repository;

        public CategoryService(CategoryRepository repository)
        {
            _repository = repository;
        }


        public async Task<Category> Created(CreateCategoryDto dto)
        {
            return await _repository.Create(dto);
        }

        public async Task<List<Category>> VerifyCategories(string[] cats)
        {
            var filter = Builders<Category>.Filter.In(
                u => u.Id,
                cats
            );
            var categories = await _repository.FindAll(filter);
            if (categories.Count != cats.Length)
                throw new NotFoundException("Las categorías proporcionadas no se encuentran registrados en nuestro sistema");
            return categories;
        }
    }
}
