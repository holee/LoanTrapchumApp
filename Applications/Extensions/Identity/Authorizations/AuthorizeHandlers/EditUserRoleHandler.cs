using System.Security.Claims;
using Applications.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Applications.Extensions.Identity.Authorizations.AuthorizeHandlers
{
    public class EditUserRoleHandler : AuthorizationHandler<ManageUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageUserRequirement requirement)
        {
            var authContextFilter = context.Resource as AuthorizationFilterContext;

            if (authContextFilter is null)
            {
                return Task.CompletedTask;
            }

            var loginUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var editUserId = authContextFilter.RouteData.Values["id"]?.ToString();

            if (loginUserId is null)
            {
                return Task.CompletedTask;
            }

            if (loginUserId != editUserId &&
                (context.User.IsInRole(AppRole.SeniorAccount.ToString()) ||
                 context.User.IsInRole(AppRole.Admin.ToString())) &&
                context.User.HasClaim(c => c.Value == "1"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

