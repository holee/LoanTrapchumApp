using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public static class ThePolicies
{
    private const string Prefix = "repayments";

    private static readonly string CanExchangedPermission = AppPermission.RuleFor(Resources.Transactions, Actions.ExchangeMoney);
    private static readonly string ReadPermission = AppPermission.RuleFor(Resources.Repayments, Actions.Read);
    private static readonly string UpdatePermission = AppPermission.RuleFor(Resources.Repayments, Actions.Update);
    private static readonly string CreatePermission = AppPermission.RuleFor(Resources.Repayments, Actions.Create);
    private static readonly string DeletePermission = AppPermission.RuleFor(Resources.Repayments, Actions.Delete);
    private static readonly string MultiplePermission = AppPermission.RuleFor(Resources.Repayments, Actions.MultiplePayment);
    private static readonly string PayOffPermission = AppPermission.RuleFor(Resources.Repayments, Actions.PayOff);
    private static readonly string RecievePaymentPermission = AppPermission.RuleFor(Resources.Repayments, Actions.ReceivePayment);
    private static readonly string WriteOffPermission = AppPermission.RuleFor(Resources.Repayments, Actions.WriteOff);

    private static readonly string[] AccountingRoles = [AppRoles.Account, AppRoles.SeniorAccount, AppRoles.ChiefAccount, AppRoles.Teller];

    public const string Create = $"{Prefix}.{nameof(Create)}";
    public const string Update = $"{Prefix}.{nameof(Update)}";
    public const string Delete = $"{Prefix}.{nameof(Delete)}";
    public const string Read = $"{Prefix}.{nameof(Read)}";
    public const string MultiplePayment = $"{Prefix}.{nameof(MultiplePayment)}";
    public const string PayOff = $"{Prefix}.{nameof(PayOff)}";
    public const string RecievePayment = $"{Prefix}:{nameof(RecievePayment)}";
    public const string WriteOff = $"{Prefix}:{nameof(WriteOff)}";
    public const string CanExchanged = $"tranx.{nameof(CanExchanged)}";
    public static AuthorizationPolicy CanExchangeMoney() => BuildPermissionPolicy(CanExchangedPermission, AccountingRoles);

    public static AuthorizationPolicy IsAccountOnly() => BuildRolePolicy(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.Teller, AppRoles.SeniorAccount);

    public static AuthorizationPolicy IsAdminOnly() => BuildRolePolicy(AppRoles.Admin, AppRoles.SuperAdmin);

    public static AuthorizationPolicy IsBranchManagerOnly() => BuildRolePolicy(AppRoles.Bm, AppRoles.Rbm, AppRoles.ChiefBm);

    public static AuthorizationPolicy IsCreditOfficerOnly() => BuildRolePolicy(AppRoles.Co);

    public static AuthorizationPolicy IsManagerOnly() => BuildRolePolicy(AppRoles.Manager, AppRoles.Ceo);

    public static AuthorizationPolicy IsCEOOnly() => BuildRolePolicy(AppRoles.Ceo);

    public static AuthorizationPolicy CanCreate() => BuildPermissionPolicy(CreatePermission);

    public static AuthorizationPolicy CanUpdate() => BuildPermissionPolicy(UpdatePermission);

    public static AuthorizationPolicy CanRead() => BuildPermissionPolicy(ReadPermission);

    public static AuthorizationPolicy CanDelete() => BuildPermissionPolicy(DeletePermission);

    public static AuthorizationPolicy CanPayMultiple() => BuildPermissionPolicy(MultiplePermission, AccountingRoles);

    public static AuthorizationPolicy CanPayOff() => BuildPermissionPolicy(PayOffPermission, AccountingRoles);

    public static AuthorizationPolicy CanRecievePayment() => BuildPermissionPolicy(RecievePaymentPermission, AccountingRoles);

    public static AuthorizationPolicy CanWriteOff() => BuildPermissionPolicy(WriteOffPermission, AccountingRoles);

    private static AuthorizationPolicy BuildRolePolicy(params string[] roles) =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(roles)
            .Build();

    private static AuthorizationPolicy BuildPermissionPolicy(string permission, params string[] roles)
    {
        var builder = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser();

        if (roles.Length > 0)
        {
            builder.RequireRole(roles);
        }

        return builder
            .RequireClaim(AppClaim.Permission, permission)
            .Build();
    }
}
