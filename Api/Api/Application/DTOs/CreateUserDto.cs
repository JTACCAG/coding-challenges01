using System.ComponentModel.DataAnnotations;

namespace Api.Application.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string Fullname { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
