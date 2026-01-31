using System.ComponentModel.DataAnnotations;

namespace Api.Modules.Auth.Dtos
{
    public class ResponseAuthDto
    {
        public string AccessToken { get; set; } = null!;
    }
}
