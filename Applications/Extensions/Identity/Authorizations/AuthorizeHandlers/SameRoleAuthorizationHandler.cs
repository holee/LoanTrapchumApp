using System.Security.Claims;
using Applications.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Applications.Responses.ViewModels.Auths;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Authorizations.AuthorizeHandlers
{
    public class SameRoleAuthorizationHandler : AuthorizationHandler<SameRoleRequirement, RoleClaims>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,SameRoleRequirement requirement,
            RoleClaims resource)
        {

            string loginRoleId = context.User.FindFirstValue("roleId")!;

            if (resource.RoleId == loginRoleId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
