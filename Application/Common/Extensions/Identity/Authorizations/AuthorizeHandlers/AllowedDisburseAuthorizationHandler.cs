using System.Security.Claims;
using Application.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Application.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeHandlers
{
    public class AllowedDisburseAuthorizationHandler : AuthorizationHandler<AllowedDisburseAuthorizationRquirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowedDisburseAuthorizationRquirement requirement)
        {

            if (context.User.HasClaim(ClaimTypes.Role, AppRoles.Account) ||
                context.User.HasClaim(ClaimTypes.Role, AppRoles.ChiefAccount) ||
                context.User.HasClaim(ClaimTypes.Role, AppRoles.Teller) ||
                context.User.HasClaim(ClaimTypes.Role, AppRoles.SeniorAccount))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
