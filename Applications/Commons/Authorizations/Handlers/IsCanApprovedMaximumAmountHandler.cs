using Applications.Commons.Authorizations.Rquirements;
using Applications.Extensions.Identity.Enumerations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Enumerations;
using Domains.Models.Loans;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Commons.Authorizations.Handlers;

public sealed class IsCanApprovedMaximumAmountHandler
    : AuthorizationHandler<IsCanApprovedMaximumAmountRequirement, VLoanRequests>
{
    private const decimal MaxNormalUsd = 250m;
    private const decimal MaxNormalKhr = 1_000_000m;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsCanApprovedMaximumAmountRequirement requirement,
        VLoanRequests resource)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
            return Task.CompletedTask;

        bool isManagerOrCeo = user.IsInRole(AppRoles.Manager)
                           || user.IsInRole(AppRoles.Ceo);

        if (!isManagerOrCeo)
            return Task.CompletedTask;

        var amount = resource.RequestAmount ?? 0m;
        var currency = (CurrencyEnum)(resource.CurrencyId ?? (int)CurrencyEnum.USD);

        bool isMaximumAmount = currency == CurrencyEnum.USD
            ? amount > MaxNormalUsd
            : amount > MaxNormalKhr;

        if (isMaximumAmount)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
