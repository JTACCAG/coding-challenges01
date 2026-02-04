using Api.Application.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Api.Application.Authorization.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleEnum Role { get; }

        public RoleRequirement(RoleEnum role)
        {
            Role = role;
        }
    }
}
