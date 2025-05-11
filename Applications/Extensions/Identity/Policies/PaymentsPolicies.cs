using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public static class PaymentsPolicies
{
    private const string prefix = "repayments";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string MultiplePayment = $"{prefix}.{nameof(MultiplePayment)}";
    public const string PayOff = $"{prefix}.{nameof(PayOff)}";

    private static string read = AppPermission.RuleFor(Modules.Repayments, Feature.Read);
    private static string update = AppPermission.RuleFor(Modules.Repayments, Feature.Update);
    private static string create = AppPermission.RuleFor(Modules.Repayments, Feature.Create);
    private static string delete = AppPermission.RuleFor(Modules.Repayments, Feature.Delete);
    private static string multiple = AppPermission.RuleFor(Modules.Repayments, Feature.MultiplePayment);
    private static string payoff = AppPermission.RuleFor(Modules.Repayments, Feature.PayOff);
    public static AuthorizationPolicy CanCreate()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, create)
                    .Build();
    }

    public static AuthorizationPolicy CanUpdate()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                     .RequireClaim(AppClaim.Permission, update)
                    .Build();
    }

    public static AuthorizationPolicy CanRead()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, read)
                    .Build();
    }

    public static AuthorizationPolicy CanDelete()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, delete)
                    .Build();
    }

    public static AuthorizationPolicy CanPayMultiple()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account)
                    .RequireClaim(AppClaim.Permission, multiple)
                    .Build();
    }


    public static AuthorizationPolicy CanPayOff()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account)
                    .RequireClaim(AppClaim.Permission, payoff)
                    .Build();
    }


}
