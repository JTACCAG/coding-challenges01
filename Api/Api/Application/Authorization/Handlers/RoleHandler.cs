using Api.Application.Authorization.Requirements;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Api.Application.Authorization.Handlers
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
                return Task.CompletedTask;

            var user = httpContext.Items["User"] as User;

            if (user == null)
                return Task.CompletedTask;

            if (user.Role != requirement.Role)
                return Task.CompletedTask;

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
