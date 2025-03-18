using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public static class LoanRequestPolicies
{
    private static string read = AppPermission.RuleFor(Modules.LoanRequest, Feature.Read);
    private static string update = AppPermission.RuleFor(Modules.LoanRequest, Feature.Update);
    private static string create = AppPermission.RuleFor(Modules.LoanRequest, Feature.Create);
    private static string delete = AppPermission.RuleFor(Modules.LoanRequest, Feature.Delete);
    private static string aprove = AppPermission.RuleFor(Modules.LoanRequest, Feature.ApproveRequest);
    private static string reject = AppPermission.RuleFor(Modules.LoanRequest, Feature.RejectRequest);

    public static AuthorizationPolicy CanDisburseLoan()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Account, AppRoles.SeniorAccount, AppRoles.ChiefAccount)
                    .RequireClaim(AppClaim.Permission, AppPermission.RuleFor(Modules.LoanRequest, Feature.Disburse))
                    .Build();
    }


    public static AuthorizationPolicy CanCreate()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Co)
                    .RequireClaim(AppClaim.Permission, create)
                    .Build();
    }

    public static AuthorizationPolicy CanRequest() 
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Co)
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

    public static AuthorizationPolicy CanAproveRequest()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(AppRoles.Manager, AppRoles.Bm, AppRoles.Rbm, AppRoles.Dbm)
                    .RequireClaim(AppClaim.Permission, aprove)
                    .Build();
    }

    public static AuthorizationPolicy CanRejectRequest()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                     .RequireRole(AppRoles.Manager, AppRoles.Bm, AppRoles.Rbm, AppRoles.Dbm)
                    .RequireClaim(AppClaim.Permission, reject)
                    .Build();
    }




}
