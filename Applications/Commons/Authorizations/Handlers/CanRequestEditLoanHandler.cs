using Applications.Commons.Authorizations.Rquirements;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Applications.Commons.Authorizations.Handlers;

public sealed class CanRequestEditLoanHandler : AuthorizationHandler<CanRequestEditLoanDisbursementRequirement, VLoanDisbursments>
{
    private static readonly string[] AllowedRoles = 
    [
        AppRoles.Account,
        AppRoles.ChiefAccount,
        AppRoles.Teller,
    ];

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanRequestEditLoanDisbursementRequirement requirement, VLoanDisbursments resource)
    {
        var user = context.User;

        if (user.Identity?.IsAuthenticated != true)
            return Task.CompletedTask;

        if (!AllowedRoles.Any(user.IsInRole))
            return Task.CompletedTask;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.Equals(userId, resource.CreatedBy, StringComparison.Ordinal))
            return Task.CompletedTask;

        if (resource.PaidPrinciple <= 0)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
