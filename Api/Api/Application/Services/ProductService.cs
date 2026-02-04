using Api.Application.DTOs;
using Api.Application.Exceptions;
using Api.Application.Repositories;
using Api.Domain.Entities;
using MongoDB.Driver;
using QuestPDF.Fluent;
using System.Text.Json;

namespace Api.Application.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository;
        private readonly ReportRepository _reportRepository;
        private readonly CategoryService _categoryService;

        public ProductService(
            ProductRepository repository,
            ReportRepository reportRepository,
            CategoryService categoryService)
        {
            _repository = repository;
            _reportRepository = reportRepository;
            _categoryService = categoryService;
        }


        public async Task<Product> Created(CreateProductDto dto)
        {
            await _categoryService.VerifyCategories(dto.CategoryIds);
            return await _repository.Create(dto);
        }

        public async Task<Product> Updated(string id, UpdateProductDto dto)
        {
            var product = await ValidateProduct(id);
            if (dto.Name is not null) product.Name = dto.Name;
            if (dto.Description is not null) product.Description = dto.Description;
            if (dto.Price is not null) product.Price = dto.Price.Value;
            if (dto.StockQuantity is not null) product.StockQuantity = dto.StockQuantity.Value;
            if (dto.CategoryIds is not null && dto.CategoryIds.Length > 0) {
                await _categoryService.VerifyCategories(dto.CategoryIds);
                product.CategoryIds = dto.CategoryIds;
            }
            return await _repository.Upsert(id,product);
        }

        public async Task<Product> Deleted(string id)
        {
            await ValidateProduct(id);
            return await _repository.Delete(id);
        }

        public async Task<Product> ValidateProduct(string id)
        {
            var product = await FindOne(id) ?? throw new NotFoundException("El producto proporcionado no se encuentra registrado en nuestro sistema");
            return product;
        }

        public async Task<Product?> FindOne(string id)
        {
            var filter = Builders<Product>.Filter.Eq(
                u => u.Id,
                id
            );
            var product = await _repository.FindOne(filter);
            return product;
        }

        public async Task<List<Product>> FindAll(SearchProductDto dto)
        {
            var products = await _repository.FindAll();
            return products;
        }

        public async Task<byte[]> GeneratePdfInventoryReport()
        {
            var reports = await _reportRepository.GetReports();
            Console.WriteLine(JsonSerializer.Serialize(reports));
            var document = new InventoryReport(reports);
            return document.GeneratePdf();
        }
    }
}
