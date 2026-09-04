using Application.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions.Identity.Policies;

public static class VendorsPolicies
{
    private const string prefix = "vendors";
    public const string Create = $"{prefix}.{nameof(Create)}";
    public const string Read = $"{prefix}.{nameof(Read)}";
    public const string Update = $"{prefix}.{nameof(Update)}";
    public const string Delete = $"{prefix}.{nameof(Delete)}";

    private static string read = AppPermission.RuleFor(Resources.Vendors, Actions.Read);
    private static string update = AppPermission.RuleFor(Resources.Vendors, Actions.Update);
    private static string create = AppPermission.RuleFor(Resources.Vendors, Actions.Create);
    private static string delete = AppPermission.RuleFor(Resources.Vendors, Actions.Delete);
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

