using Applications.Interfaces.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace Infrastructures.Implements.Identity;

public sealed class CurrentUserReposity : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserReposity(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal? User => _accessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public string? UserId => IsAuthenticated ? User!.FindFirstValue(ClaimTypes.NameIdentifier) : null;

    public string? UserName => IsAuthenticated ? User!.Identity!.Name : null;

    public IEnumerable<Claim> Claims => User?.Claims ?? Enumerable.Empty<Claim>();

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}
