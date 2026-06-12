using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;
 
public partial class Policies 
{
    public const string TranxCreate = $"tranx.{nameof(TranxCreate)}";
    public const string TranxRead = $"tranx.{nameof(TranxRead)}";
    public const string TranxUpdate = $"tranx.{nameof(TranxUpdate)}";
    public const string TranxDelete = $"tranx.{nameof(TranxDelete)}";
    public const string TranxVaultIn = $"tranx.{nameof(TranxVaultIn)}";
    public const string TranxVaultOut = $"tranx.{nameof(TranxVaultOut)}";
    public const string OpenBranch = $"tranx.{nameof(OpenBranch)}";
    public const string CloseBranch = $"tranx.{nameof(CloseBranch)}";
    private static string tranx_read = AppPermission.RuleFor(Resources.Transactions, Actions.Read);
    private static string tranx_update = AppPermission.RuleFor(Resources.Transactions, Actions.Update);
    private static string tranx_create = AppPermission.RuleFor(Resources.Repayments, Actions.Create);
    private static string tranx_delete = AppPermission.RuleFor(Resources.Transactions, Actions.Delete);
    private static string tranx_vualtIn = AppPermission.RuleFor(Resources.Transactions, Actions.VaultIn);
    private static string tranx_vualtOut = AppPermission.RuleFor(Resources.Transactions, Actions.VaultOut);
    private static string tranx_closedBranch = AppPermission.RuleFor(Resources.Transactions, Actions.CloseBranch);
    private static string tranx_openedBranch = AppPermission.RuleFor(Resources.Transactions, Actions.OpenBranch);
    public static AuthorizationPolicy TranxCanCreate()  
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, tranx_create)
                    .Build();
    }

    public static AuthorizationPolicy TranxCanUpdate()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                     .RequireClaim(AppClaim.Permission, tranx_update)
                    .Build();
    }

    public static AuthorizationPolicy TranxCanRead()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, tranx_read)
                    .Build();
    }

    public static AuthorizationPolicy TranxCanDelete()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, tranx_delete)
                    .Build();
    }

    public static AuthorizationPolicy CanVaultOut()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.SeniorAccount)
                    .RequireClaim(AppClaim.Permission, tranx_vualtOut)
                    .Build();
    }

    public static AuthorizationPolicy CanVaultIn()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.ChiefAccount, AppRoles.SeniorAccount)
                    .RequireClaim(AppClaim.Permission, tranx_vualtIn)
                    .Build();
    }

    public static AuthorizationPolicy CanOpenBranch()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, tranx_openedBranch)
                    .Build();
    }

    public static AuthorizationPolicy CanClosedBranch()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, tranx_closedBranch)
                    .Build();
    }



}

