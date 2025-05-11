using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public static class TranxsPolicies
{
    private const string prefix = "tranx";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    public const string VaultIn = $"{prefix}.{nameof(VaultIn)}";
    public const string VaultOut = $"{prefix}.{nameof(VaultOut)}";
    public const string OpenBranch = $"{prefix}.{nameof(OpenBranch)}";
    public const string CloseBranch = $"{prefix}.{nameof(CloseBranch)}";

    private static string read = AppPermission.RuleFor(Modules.Transactions, Feature.Read);
    private static string update = AppPermission.RuleFor(Modules.Transactions, Feature.Update);
    private static string create = AppPermission.RuleFor(Modules.Repayments, Feature.Create);
    private static string delete = AppPermission.RuleFor(Modules.Transactions, Feature.Delete);
    private static string vualtIn = AppPermission.RuleFor(Modules.Transactions, Feature.VaultIn);
    private static string vualtOut = AppPermission.RuleFor(Modules.Transactions, Feature.VaultOut);
    private static string closedBranch = AppPermission.RuleFor(Modules.Transactions, Feature.CloseBranch);
    private static string openedBranch = AppPermission.RuleFor(Modules.Transactions, Feature.OpenBranch);
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

    public static AuthorizationPolicy CanVaultOut()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.SeniorAccount)
                    .RequireClaim(AppClaim.Permission, vualtOut)
                    .Build();
    }

    public static AuthorizationPolicy CanVaultIn()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.SeniorAccount)
                    .RequireClaim(AppClaim.Permission, vualtIn)
                    .Build();
    }

    public static AuthorizationPolicy CanOpenBranch()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, openedBranch)
                    .Build();
    }

    public static AuthorizationPolicy CanClosedBranch()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, closedBranch)
                    .Build();
    }



}

