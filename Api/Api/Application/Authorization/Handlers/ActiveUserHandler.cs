using Api.Application.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Api.Application.Authorization.Handlers
{
    public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
        {
            // Ejemplo: validar claim
            var isActive = context.User.HasClaim("isActive", "true");

            if (isActive)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
