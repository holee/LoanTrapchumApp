using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public static class EmployeesPolicies
{
    private const string prefix = "employees";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";
    private static string read = AppPermission.RuleFor(Modules.Employees, Feature.Read);
    private static string update = AppPermission.RuleFor(Modules.Employees, Feature.Update);
    private static string create = AppPermission.RuleFor(Modules.Employees, Feature.Create);
    private static string delete = AppPermission.RuleFor(Modules.Employees, Feature.Delete);
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
