using Application.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeHandlers
{
    public class SameUserAuthorizationHandler : AuthorizationHandler<SameUserRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            SameUserRequirement requirement)
        {

            //string loginRoleId = context.User.FindFirstValue("roleId")!;
            //if(long.TryParse(loginRoleId, out long roleId)) 
            //{
            //    if (resource.RoleId == roleId)
            //    {
            //        context.Succeed(requirement);
            //    }
            //}
            return Task.CompletedTask;
        }
    }
}
