using Api.Application.Enums;
using Api.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Fullname { get; set; } = null!;

        public RoleEnum Role { get; set; } = RoleEnum.Regular;
    }

    public class ResponseAuthDto
    {
        public string AccessToken { get; set; } = null!;
        public UserDto User { get; set; } = null!;
    }
}
