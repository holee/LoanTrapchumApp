using Application.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Application.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeHandlers
{
    public class CreateUserHandler : AuthorizationHandler<ManageUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
                           ManageUserRequirement requirement)
        {

            if (context.User.IsInRole(AppRole.SuperAdmin.ToString()) || 
                context.User.IsInRole(AppRole.Admin.ToString()))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}

//var permissions = context.User.Claims
//                                .Where(x => x.Type == "permissions")
//                                .Select(x => x.Value)
//                                .ToHashSet();

//if(permissions!.Any() && permissions.Contains(requirement!.ToString())) {

//}
//var authFilterContext = context.Resource as AuthorizationFilterContext;

//if (authFilterContext is null)
//{
//    return Task.CompletedTask;
//}
//var permissions=authFilterContext.HttpContext.us