using Microsoft.AspNetCore.Authorization;

namespace Tutoring_Platform.CustomModels
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            try
            {
                var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');
                if (scopes.Any(s => s == requirement.Scope))
                    context.Succeed(requirement);
            }
            catch(Exception e)
            {}

            // Succeed if the scope array contains the required scope
            return Task.CompletedTask;
        }
    }
}
