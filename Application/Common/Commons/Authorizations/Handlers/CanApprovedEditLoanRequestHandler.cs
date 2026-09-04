using Application.Commons.Authorizations.Rquirements;
using Application.Constants.Authorizations;
using Application.Extensions.Identity.Enumerations;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Authorization;

namespace Application.Commons.Authorizations.Handlers;

public sealed class CanApprovedEditLoanRequestHandler : AuthorizationHandler<CanApprovedEditLoanRequestRequirement, LoanDisbursment>
{

    private static readonly string[] AllowedRoles =
    [
        AppRoles.Manager,
        AppRoles.Ceo
    ];

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
         CanApprovedEditLoanRequestRequirement requirement, LoanDisbursment resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
            return Task.CompletedTask;


        bool hasAllowedRole = AllowedRoles.Any(user.IsInRole);

        if (!hasAllowedRole)
            return Task.CompletedTask;

        var requiredPermission = AppPermission.RuleFor(Resources.Loans, Actions.Update);

        if (!user.HasClaim(AppClaim.Permission, requiredPermission))
            return Task.CompletedTask;

        if (resource.RequestEdit)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

