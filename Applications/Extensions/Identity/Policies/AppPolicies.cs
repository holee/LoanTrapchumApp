using Applications.Constants.Authorizations;
using Applications.Extensions.Identity.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;


public static class AppPolicies
{
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
                    .RequireRole(AppRoles.Manager, AppRoles.Dbm, AppRoles.Rbm, AppRoles.Bm)
                    .Build();
    }


}
