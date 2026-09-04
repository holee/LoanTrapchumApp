using Applications.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructures.Repository;

public sealed class CurrentUserRepository : ICurrentUserServices
{
    private readonly IHttpContextAccessor _accessor;
    public CurrentUserRepository(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    private ClaimsPrincipal? User => _accessor.HttpContext?.User;
    public string? UserId => GetClaimValue(ClaimTypes.NameIdentifier);
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public string? UserName => User?.Identity?.Name;
    public string? Email => GetClaimValue(ClaimTypes.Email);
    public string? Role => GetClaimValue(ClaimTypes.Role);
    public string? GetUserId() => GetClaimValue(ClaimTypes.NameIdentifier);
    public string? GetUserName() => User?.Identity?.Name;
    public string? GetEmail() => GetClaimValue(ClaimTypes.Email);
    public string? GetRole() => GetClaimValue(ClaimTypes.Role);
    public IReadOnlyList<string> Roles =>
                        User?
                        .FindAll(ClaimTypes.Role)
                        .Select(x => x.Value)
                        .ToList()
                    ?? [];

    // 🔹 Generic helper (reusable)
    private string? GetClaimValue(string claimType) => User?.FindFirst(claimType)?.Value;
}
