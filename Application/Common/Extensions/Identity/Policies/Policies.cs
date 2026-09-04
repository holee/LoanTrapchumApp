using Application.Constants.Authorizations;
using Application.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Policies;

public partial class Policies
{
    private static string _canExchanged = AppPermission.RuleFor(Resources.Transactions, Actions.ExchangeMoney);
    private const string prefix = "repayments";
    private static string read = AppPermission.RuleFor(Resources.Repayments, Actions.Read);
    private static string update = AppPermission.RuleFor(Resources.Repayments, Actions.Update);
    private static string create = AppPermission.RuleFor(Resources.Repayments, Actions.Create);
    private static string delete = AppPermission.RuleFor(Resources.Repayments, Actions.Delete);
    private static string multiple = AppPermission.RuleFor(Resources.Repayments, Actions.MultiplePayment);
    private static string payoff = AppPermission.RuleFor(Resources.Repayments, Actions.PayOff);
    private static string recievePayment = AppPermission.RuleFor(Resources.Repayments, Actions.ReceivePayment);
    private static string writeOff = AppPermission.RuleFor(Resources.Repayments, Actions.WriteOff);
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string MultiplePayment = $"{prefix}.{nameof(MultiplePayment)}";
    public const string PayOff = $"{prefix}.{nameof(PayOff)}";
    public const string RecievePayment = $"{prefix}:{nameof(RecievePayment)}";
    public const string WriteOff = $"{prefix}:{nameof(WriteOff)}";
    public const string CanExchanged = $"tranx.{nameof(CanExchanged)}";
    public static AuthorizationPolicy CanExchangeMoney()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.SeniorAccount, AppRoles.ChiefAccount, AppRoles.Teller)
                    .RequireClaim(AppClaim.Permission, _canExchanged)
                    .Build();

    }


    public static AuthorizationPolicy IsAccountOnly()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.Teller, AppRoles.SeniorAccount)
                    .Build();
    }

    public static AuthorizationPolicy IsAdminOnly()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Admin, AppRoles.SuperAdmin)
                    .Build();
    }

    public static AuthorizationPolicy IsBranchManagerOnly()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Bm, AppRoles.Rbm, AppRoles.ChiefBm)
                    .Build();
    }

    public static AuthorizationPolicy IsCreditOfficerOnly()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Co)
                    .Build();
    }

    public static AuthorizationPolicy IsManagerOnly()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Manager, AppRoles.Ceo)
                    .Build();
    }

    public static AuthorizationPolicy IsCEOOnly()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Ceo)
                    .Build();
    }





   
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
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.SeniorAccount, AppRoles.Teller)
                    .RequireClaim(AppClaim.Permission, multiple)
                    .Build();
    }


    public static AuthorizationPolicy CanPayOff()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.SeniorAccount, AppRoles.Teller)
                    .RequireClaim(AppClaim.Permission, payoff)
                    .Build();
    }

    public static AuthorizationPolicy CanRecievePayment()
    {

        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.SeniorAccount, AppRoles.Teller)
                    .RequireClaim(AppClaim.Permission, recievePayment)
                    .Build();
    }

    public static AuthorizationPolicy CanWriteOff()
    {

        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.SeniorAccount, AppRoles.Teller)
                    .RequireClaim(AppClaim.Permission, writeOff)
                    .Build();
    }


}
