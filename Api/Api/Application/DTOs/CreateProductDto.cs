using Api.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;


        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }


        [Required]
        public int StockQuantity { get; set; }

        [Required]
        [MinLength(1)]
        [MongoObjectIdArray]
        public string[] CategoryIds { get; set; } = null!;
    }
}
