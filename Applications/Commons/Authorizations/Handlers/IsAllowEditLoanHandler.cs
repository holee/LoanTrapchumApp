using Applications.Commons.Authorizations.Rquirements;
using Applications.Extensions.Identity.Enumerations;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Commons.Authorizations.Handlers;

public sealed class IsAllowEditLoanHandler : AuthorizationHandler<IsAllowEditLoanRequirement, LoanDisbursment>
{
    private static readonly string[] AllowedRoles =
    [
        AppRoles.Account,
        AppRoles.ChiefAccount,
        AppRoles.Teller,
    ];
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAllowEditLoanRequirement requirement, LoanDisbursment resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return Task.CompletedTask;

        }

        bool hasAllowedRole = AllowedRoles.Any(user.IsInRole);

        if (!hasAllowedRole)
            return Task.CompletedTask;


        if (resource.AllowEdit)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
