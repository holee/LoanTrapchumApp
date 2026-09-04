using Application.Constants.Authorizations;
using Application.Extensions.Identity.Authorizations.AuthorizeRequirements;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Authorizations.AuthorizeHandlers
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override  async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User is null)
            {
                await Task.CompletedTask;
            }
            var permissions = context.User!.Claims
                                     .Where(x => x.Type == AppClaim.Permission && x.Value == requirement.Permission)
                                     .ToList();
            if (permissions.Any())
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
           
        }
    }
}

//private readonly IServiceScopeFactory _serviceScopeFactory;

//public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
//{
//    _serviceScopeFactory = serviceScopeFactory;
//}

//var roleId = context.User.Claims.Where(x => x.Type == "roleId")
//                            .Select(x => x.Value)
//                            .FirstOrDefault();

//using IServiceScope scope = _serviceScopeFactory.CreateScope();

//IPermissionService roleManager = scope.ServiceProvider!
//                                        .GetService<IPermissionService>()!;

//var ss =await roleManager.GetPermission(roleId!);