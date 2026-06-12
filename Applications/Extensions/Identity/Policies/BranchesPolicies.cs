using Applications.Constants.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Applications.Extensions.Identity.Policies;

public partial class Policies
{
    private const string br_prefix = "branches";
    public const string BranchCreate = $"{br_prefix}.{nameof(BranchCreate)}";
    public const string BranchRead = $"{br_prefix}.{nameof(BranchRead)}";
    public const string BranchUpdate = $"{br_prefix}.{nameof(BranchUpdate)}";
    public const string BranchDelete = $"{br_prefix}.{nameof(BranchDelete)}";
    public const string BranchShowBranch = $"{br_prefix}.{nameof(BranchShowBranch)}";

    private static string br_read = AppPermission.RuleFor(Resources.BranchesDashboard, Actions.Read);
    private static string br_update = AppPermission.RuleFor(Resources.BranchesDashboard, Actions.Update);
    private static string br_create = AppPermission.RuleFor(Resources.BranchesDashboard, Actions.Create);
    private static string br_delete = AppPermission.RuleFor(Resources.BranchesDashboard, Actions.Delete);

    public static AuthorizationPolicy CanCreateBranch()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, br_create)
                    .Build();
    }

    public static AuthorizationPolicy CanUpdateBranch()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                     .RequireClaim(AppClaim.Permission, br_update)
                    .Build();
    }

    public static AuthorizationPolicy CanReadBranch()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, br_read)
                    .Build();
    }

    public static AuthorizationPolicy CanDeleteBranch()
    {
        return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(AppClaim.Permission, br_delete)
                    .Build();
    }





}
