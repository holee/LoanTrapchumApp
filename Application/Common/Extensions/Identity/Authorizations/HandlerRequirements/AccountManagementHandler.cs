using Application.Constants.Authorizations;
using Application.Extensions.Identity.Authorizations.Requirements;
using Microsoft.AspNetCore.Authorization;
namespace Application.Extensions.Identity.Authorizations.HandlerRequirements
{
    public class AccountManagementHandler : AuthorizationHandler<AccountDashboardManagementRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountDashboardManagementRequirement requirement)
        {
            var auth = context.User;

            if (auth is not null && !auth.Identity!.IsAuthenticated) 
            {
                return Task.CompletedTask;
            }


            var permissions = context.User!.Claims
                                     .Where(x => x.Type ==AppClaim.Permission && x.Value == requirement.Permission)
                                     .ToList();

            if (permissions.Any())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
