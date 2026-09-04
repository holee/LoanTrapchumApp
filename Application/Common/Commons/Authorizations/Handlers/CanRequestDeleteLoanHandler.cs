using Application.Commons.Authorizations.Rquirements;
using Application.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Application.Commons.Authorizations.Handlers;

public sealed class CanRequestDeleteLoanHandler : AuthorizationHandler<CanRequestDeleteLoanDisbursementRequirement, VLoanDisbursments>
{
    //private readonly IServiceScopeFactory _scopeFactory;

    //public CanDeleteLoanHandler(IServiceScopeFactory serviceProvider)
    //{
    //    _scopeFactory = serviceProvider;
    //}

    private static readonly string[] AllowedRoles =
    [
        AppRoles.Account,
        AppRoles.ChiefAccount,
        AppRoles.Teller,
    ];

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanRequestDeleteLoanDisbursementRequirement requirement, VLoanDisbursments resource)
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
