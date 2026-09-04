using Application.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace Application.Extensions.Identity.Authorizations.Extensions;

public static class AuthorizationServiceExtension
{

    public static async Task<bool> MustHasPermissionAsync(this IAuthorizationService service, ClaimsPrincipal user, string resource, string action)
    {
        return (await service.AuthorizeAsync(user, null, AppPermission.RuleFor(resource, action))).Succeeded;
    }

    public static async Task<bool> MustHasPermissionAsync(this IAuthorizationService service, ClaimsPrincipal user, string policyName)
    {
        return (await service.AuthorizeAsync(user, null, policyName)).Succeeded;
    }
}
