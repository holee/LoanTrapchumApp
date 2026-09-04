using Application.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeHandlers
{

    public class SuperAdminUserHandler : AuthorizationHandler<ManageUserRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageUserRequirement requirement)
        {

            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
