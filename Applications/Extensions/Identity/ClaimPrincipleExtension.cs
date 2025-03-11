using System.Security.Claims;

namespace Applications.Extensions.Identity;

public static class ClaimPrincipleExtension
{
    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
    }

    public static string? GetUserName(this ClaimsPrincipal claimsPrincipal)
                       => claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
    public static long GetId(this ClaimsPrincipal claimsPrincipal)
    {
        var value = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (long.TryParse(value, out var id))
        {
            return id;
        }

        return default;
    }

    public static string? GetRole(this ClaimsPrincipal claimsPrincipal)
    {
        var role = claimsPrincipal.Claims.Where(x => x.Type == "roles")
                                        .Select(x => x.Value)
                                        .FirstOrDefault();
        return role;
    }

    public static HashSet<string>? GetPermissions(this ClaimsPrincipal claimsPrincipal)
    {
        var permissions = claimsPrincipal.Claims.Where(x => x.Type == "permissions")
                                        .Select(x => x.Value)
                                        .ToHashSet();
        return permissions;
    }
}
