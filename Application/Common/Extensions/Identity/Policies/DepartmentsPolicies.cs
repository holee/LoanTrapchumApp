using Application.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Policies;

public static class DepartmentsPolicies
{
    private static string read = AppPermission.RuleFor(Resources.Departments, Actions.Read);
    private static string update = AppPermission.RuleFor(Resources.Departments, Actions.Update);
    private static string create = AppPermission.RuleFor(Resources.Departments, Actions.Create);
    private static string delete = AppPermission.RuleFor(Resources.Departments, Actions.Delete);
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
