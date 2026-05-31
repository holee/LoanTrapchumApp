using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace Applications.Extensions.Identity.Authorizations.Extensions;

public static class AuthorizationServiceExtension
{

    public static async Task<bool> MustHasPermissionAsync(this IAuthorizationService service, ClaimsPrincipal user, string module, string action)
    {
        return (await service.AuthorizeAsync(user, null, AppPermission.RuleFor(module, action))).Succeeded;
    }

    public static async Task<bool> MustHasPermissionAsync(this IAuthorizationService service, ClaimsPrincipal user, string policyName)
    {
        return (await service.AuthorizeAsync(user, null, policyName)).Succeeded;
    }
}
