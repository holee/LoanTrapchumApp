using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public static class BranchesPolicies
{
    private static string read = AppPermission.RuleFor(Modules.BranchesDashboard, Feature.Read);
    private static string update = AppPermission.RuleFor(Modules.BranchesDashboard, Feature.Update);
    private static string create = AppPermission.RuleFor(Modules.BranchesDashboard, Feature.Create);
    private static string delete = AppPermission.RuleFor(Modules.BranchesDashboard, Feature.Delete);

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
