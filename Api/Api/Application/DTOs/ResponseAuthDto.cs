using Api.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.DTOs
{
    public class ResponseAuthDto
    {
        public string AccessToken { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
