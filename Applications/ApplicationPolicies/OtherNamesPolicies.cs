using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.ApplicationPolicies;

public static class OtherNamesPolicies
{
    private static string read = AppPermission.RuleFor(Modules.OtherNames, Feature.Read);
    private static string update = AppPermission.RuleFor(Modules.OtherNames, Feature.Update);
    private static string create = AppPermission.RuleFor(Modules.OtherNames, Feature.Create);
    private static string delete = AppPermission.RuleFor(Modules.OtherNames, Feature.Delete);
    public static AuthorizationPolicy CanCreate()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, create)
                    .Build();
    }

    public static AuthorizationPolicy CanEdit()
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
