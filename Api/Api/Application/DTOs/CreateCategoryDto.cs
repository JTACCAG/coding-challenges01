using System.ComponentModel.DataAnnotations;

namespace Api.Application.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }
}
