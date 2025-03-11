using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.ApplicationPolicies;

public static class CustomersPolicies
{
    private static string read = AppPermission.RuleFor(Modules.Customers, Feature.Read);
    private static string update = AppPermission.RuleFor(Modules.Customers, Feature.Update);
    private static string create = AppPermission.RuleFor(Modules.Customers, Feature.Create);
    private static string delete = AppPermission.RuleFor(Modules.Customers, Feature.Delete);
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

}
