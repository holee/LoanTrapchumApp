using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;
using TrapchumApp.Authorizations.Rquirements;

namespace TrapchumApp.Authorizations.Handlers;

public sealed class ManagerLoanDisbursementHandler : AuthorizationHandler<ManageLoanDisbursementRequirement>
{
    private static readonly string[] AllowedRoles =
            [
                AppRoles.Teller,
                AppRoles.SeniorAccount,
                AppRoles.ChiefAccount,
                AppRoles.Account
            ];

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageLoanDisbursementRequirement requirement)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
            return Task.CompletedTask;

        bool hasAllowedRole = AllowedRoles.Any(context.User.IsInRole);

        if (!hasAllowedRole)
        {
            return Task.CompletedTask;

        }

        var requiredPermission = AppPermission.RuleFor(Resources.LoanRequest, Actions.Disburse);

        if (context.User.HasClaim(AppClaim.Permission, requiredPermission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
