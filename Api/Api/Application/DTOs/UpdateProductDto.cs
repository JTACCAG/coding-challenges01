using Api.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.DTOs
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(1, int.MaxValue)]
        public int? StockQuantity { get; set; }

        [MongoObjectIdArray(IsRequired = false)]
        public string[]? CategoryIds { get; set; }
    }
}
