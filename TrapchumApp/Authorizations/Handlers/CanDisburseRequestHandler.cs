using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace TrapchumApp.Authorizations.Rquirements;

public sealed class CanDisburseRequestHandler : AuthorizationHandler<CanDisburseRequestRequirement, VLoanRequests>
{
    private static readonly string[] AllowedRoles =
    [
        AppRoles.Teller,
        AppRoles.SeniorAccount,
        AppRoles.ChiefAccount,
        AppRoles.Account
    ];

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanDisburseRequestRequirement requirement,
        VLoanRequests resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
            return Task.CompletedTask;

        bool hasAllowedRole = AllowedRoles.Any(user.IsInRole);

        if (!hasAllowedRole)
        {
            return Task.CompletedTask;

        }

        var requiredPermission = AppPermission.RuleFor(Modules.LoanRequest, Actions.Disburse);

        if (!user.HasClaim(AppClaim.Permission, requiredPermission))
        {
            return Task.CompletedTask;
        }

        if (resource.StatusId == (int)LoanRequestStatus.Approved)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

