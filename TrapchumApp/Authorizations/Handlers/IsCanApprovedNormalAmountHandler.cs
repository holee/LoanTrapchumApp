using Applications.Extensions.Identity.Enumerations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Authorization;
using TrapchumApp.Authorizations.Rquirements;

namespace TrapchumApp.Authorizations.Handlers;

/// <summary>
/// BM (Branch Manager) can approve loan requests with normal amounts:
/// USD <= 250 or KHR <= 1,000,000.
/// Manager and CEO can also approve normal amounts.
/// </summary>
public sealed class IsCanApprovedNormalAmountHandler
    : AuthorizationHandler<IsCanApprovedNormalAmountRequirement, VLoanRequests>
{
    private const decimal MaxNormalUsd = 250m;
    private const decimal MaxNormalKhr = 1_000_000m;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        IsCanApprovedNormalAmountRequirement requirement,
        VLoanRequests resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
            return Task.CompletedTask;

        bool isBmOrAbove = user.IsInRole(AppRoles.Rbm)
                        || user.IsInRole(AppRoles.Bm)
                        || user.IsInRole(AppRoles.ChiefBm);

        if (!isBmOrAbove)
            return Task.CompletedTask;

        var amount = resource.RequestAmount ?? 0m;

        var currency = (CurrencyEnum)(resource.CurrencyId ?? (int)CurrencyEnum.USD);

        bool isNormalAmount = currency == CurrencyEnum.USD
            ? amount <= MaxNormalUsd
            : amount <= MaxNormalKhr;

        if (isNormalAmount)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
