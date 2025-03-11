
using Applications.Extensions.Identity;
using Domains.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Applications.Extensions.Identity.Authorizations.Services;

public sealed class PermissionService : IPermissionService
{

    private readonly RoleManager<ApplicationRole> roleManager;

    public PermissionService(RoleManager<ApplicationRole> roleManager)
    {
        this.roleManager = roleManager;
    }

    public async Task<HashSet<string>> GetPermission(string roleId)
    {
        var role =await roleManager.FindByIdAsync(roleId);

        if (role is null) return [];
        var permissions = await roleManager.GetClaimsAsync(role);
        return permissions.Select(x=>x.Type).ToHashSet();
    }
}
