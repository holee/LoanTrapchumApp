using Application.Commons.Authorizations.Rquirements;
using Application.Constants.Authorizations;
using Application.Enumerations;
using Application.Extensions.Identity.Enumerations;
using Application.Features.Settings.Dto;
using Application.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Application.Commons.Authorizations.Handlers;

public sealed class CanApprovedRequestHandler : AuthorizationHandler<CanApprovedRequestRequirement, (VLoanRequests Request, VUserLoanSizes UserLoanSize)>
{

    private static readonly string[] AllowedRoles =
    [
        AppRoles.Manager,
        AppRoles.Ceo,
        AppRoles.Bm,
        AppRoles.ChiefBm,
        AppRoles.Rbm,
        AppRoles.Dbm,
    ];

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
         CanApprovedRequestRequirement requirement,
        (VLoanRequests Request, VUserLoanSizes UserLoanSize) resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
            return Task.CompletedTask;


        bool hasAllowedRole = AllowedRoles.Any(user.IsInRole);

        if (!hasAllowedRole)
            return Task.CompletedTask;

        var requiredPermission = AppPermission.RuleFor(Resources.LoanRequest, Actions.ApproveRequest);

        if (!user.HasClaim(AppClaim.Permission, requiredPermission))
            return Task.CompletedTask;

        var request = resource.Request;

        var loanSize = resource.UserLoanSize;

        if (request is null || loanSize is null)
            return Task.CompletedTask;

        var amount = request.RequestAmount ?? 0m;

        var currency = (CurrencyEnum)(request.CurrencyId ?? (int)CurrencyEnum.USD);

        bool isWithinLimit = currency == CurrencyEnum.USD
            ? amount >= (loanSize.MinUsdLoanSize ?? 0m) && amount <= (loanSize.MaxUsdLoanSize ?? 0m)
            : amount >= (loanSize.MinKhrLoanSize ?? 0m) && amount <= (loanSize.MaxKhrLoanSize ?? 0m);

        if (isWithinLimit && request.StatusId == (int)RequestStatus.Requested)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

