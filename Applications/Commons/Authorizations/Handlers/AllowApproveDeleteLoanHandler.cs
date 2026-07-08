using Applications.Commons.Authorizations.Rquirements;
using Applications.Extensions.Identity.Enumerations;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Commons.Authorizations.Handlers;

public sealed class AllowApproveDeleteLoanHandler : AuthorizationHandler<AllowApproveDeleteLoanRequirement, LoanDisbursment>
{
    private static readonly string[] AllowedRoles =
    [
        AppRoles.Manager,
        AppRoles.Ceo,
        AppRoles.Bm,
    ];
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowApproveDeleteLoanRequirement requirement, LoanDisbursment resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return Task.CompletedTask;

        }

        bool hasAllowedRole = AllowedRoles.Any(user.IsInRole);

        if (!hasAllowedRole)
            return Task.CompletedTask;

        if (resource.RequestDelete)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
