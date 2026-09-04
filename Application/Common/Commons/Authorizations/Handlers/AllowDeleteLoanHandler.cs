using Application.Commons.Authorizations.Rquirements;
using Application.Constants.Authorizations;
using Application.Extensions.Identity.Enumerations;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Authorization;

namespace Application.Commons.Authorizations.Handlers;

public sealed class AllowDeleteLoanHandler : AuthorizationHandler<AllowDeleteLoanRequirement, LoanDisbursment>
{
    private static readonly string[] AllowedRoles =
   [
        AppRoles.Account,
        AppRoles.ChiefAccount,
        AppRoles.Teller,
    ];
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowDeleteLoanRequirement requirement, LoanDisbursment resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            return Task.CompletedTask;

        }

        bool hasAllowedRole = AllowedRoles.Any(user.IsInRole);

        if (!hasAllowedRole)
            return Task.CompletedTask;

        var requiredPermission = AppPermission.RuleFor(Resources.Loans, Actions.Delete);

        if (!user.HasClaim(AppClaim.Permission, requiredPermission))
        {
            return Task.CompletedTask;
        }

        if (resource.AllowDelete)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
