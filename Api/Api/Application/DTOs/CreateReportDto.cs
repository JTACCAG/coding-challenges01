using System.ComponentModel.DataAnnotations;

namespace Api.Application.DTOs
{
    public class CreateReportDto
    {
        [Required]
        public string ProductId { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string Reason { get; set; } = null!;

        public bool Solved { get; set; } = false;
    }
}
