using System.Security.Claims;

namespace Applications.Interfaces.Identity;

public interface ICurrentUserService
{
    /// <summary>Returns <c>true</c> if the request is authenticated.</summary>
    bool IsAuthenticated { get; }

    /// <summary>The authenticated user’s GUID (Identity primary key) or <c>null</c>.</summary>
    string? UserId { get; }

    /// <summary>The user name (usually the e-mail) or <c>null</c>.</summary>
    string? UserName { get; }

    /// <summary>All claims for the current principal (empty list when anonymous).</summary>
    IEnumerable<Claim> Claims { get; }

    /// <summary>Convenience wrapper around <see cref="ClaimsPrincipal.IsInRole"/>.</summary>
    bool IsInRole(string role);
}

