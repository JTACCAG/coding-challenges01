using Api.Application.Authorization.Requirements;
using Api.Application.Repositories;
using Api.Application.Services;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Application.Authorization.Handlers
{
    public class ActiveUserHandler(UserService _userService, IHttpContextAccessor _httpContextAccessor) : AuthorizationHandler<ActiveUserRequirement>
    {
        protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(userId);
            if (string.IsNullOrWhiteSpace(userId))
                return;
            var user = await _userService.FindOne(userId);

            if (user is null)
                return;

            _httpContextAccessor.HttpContext!.Items["User"] = user;

            context.Succeed(requirement);
        }
    }
}
