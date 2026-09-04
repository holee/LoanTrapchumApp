using Application.Commons.Authorizations.Rquirements;
using Application.Constants.Authorizations;
using Application.Extensions.Identity.Enumerations;
using Application.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Application.Commons.Authorizations.Handlers;

public sealed class CanRequestHandler : AuthorizationHandler<CanRequestRequirement, VLoanRequests>
{
    private static readonly string[] AllowedRoles =
    [
        AppRoles.Co,
    ];

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CanRequestRequirement requirement,
        VLoanRequests resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
            return Task.CompletedTask;

        bool hasAllowedRole = AllowedRoles.Any(user.IsInRole);

        if (!hasAllowedRole)
            return Task.CompletedTask;

        var requiredPermission = AppPermission.RuleFor(Resources.LoanRequest, Actions.Request);

        if (!user.HasClaim(AppClaim.Permission, requiredPermission))
            return Task.CompletedTask;

        if (resource.StatusId == (int)LoanRequestStatus.New ||
            resource.StatusId == (int)LoanRequestStatus.Recheck)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

