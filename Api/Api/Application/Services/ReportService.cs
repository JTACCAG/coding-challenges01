using Api.Application.DTOs;
using Api.Application.Exceptions;
using Api.Application.Repositories;
using Api.Domain.Entities;
using Api.Domain.ValueObjects;
using MongoDB.Driver;

namespace Api.Application.Services
{
    public class ReportService
    {
        private readonly ReportRepository _reportRepository;
        private readonly ProductService _productService;

        public ReportService(ReportRepository repository, ProductService productService)
        {
            _reportRepository = repository;
            _productService = productService;
        }


        public async Task<Report?> Create(CreateReportDto dto)
        {
            await _productService.ValidateProduct(dto.ProductId);
            var report = await FindOneByProductId(dto.ProductId);
            if (report is not null) throw new BadRequestException("El producto ya fue reportado");
            return await _reportRepository.Create(dto);
        }

        public async Task<Report?> FindOneByProductId(string id)
        {
            var filter = Builders<Report>.Filter.And(
                Builders<Report>.Filter.Eq(u => u.ProductId, id),
                Builders<Report>.Filter.Eq(u => u.Solved, false)
            );
            var product = await _reportRepository.FindOne(filter);
            return product;
        }

        public async Task<List<ReportWithProductDto>> FindAllTree()
        {
            var products = await _reportRepository.GetReports();
            return products;
        }
    }
}
