using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace TrapchumApp.Authorizations.Rquirements;

public sealed class CanReceivePaymentHandler : AuthorizationHandler<CanReceievePaymentRequirement>
{
    private static readonly string[] AllowedRoles =
    [
        AppRoles.Teller,
        AppRoles.SeniorAccount,
        AppRoles.ChiefAccount,
        AppRoles.Account,
    ];

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanReceievePaymentRequirement requirement)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
            return Task.CompletedTask;

        bool hasAllowedRole = AllowedRoles.Any(user.IsInRole);

        if (!hasAllowedRole)
            return Task.CompletedTask;

        var requiredPermission = AppPermission.RuleFor(Modules.Repayments, Actions.ReceivePayment);

        if (user.HasClaim(AppClaim.Permission, requiredPermission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

