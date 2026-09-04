using Domains.Models.Identity;

namespace Web.Api.Authentication;

/// <summary>Result of issuing an access token.</summary>
public sealed record AccessTokenResult(string Token, DateTime ExpiresAtUtc, IReadOnlyList<string> Roles);

/// <summary>Result of issuing a refresh token.</summary>
public sealed record RefreshTokenResult(string Token, DateTime ExpiresAtUtc);

/// <summary>Issues and manages JWT access/refresh tokens for API clients.</summary>
public interface IJwtTokenService
{
    /// <summary>Builds a signed JWT containing the user's identity, role and permission claims.</summary>
    Task<AccessTokenResult> GenerateAccessTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    /// <summary>Creates a new refresh token and persists it against the user.</summary>
    Task<RefreshTokenResult> IssueRefreshTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    /// <summary>Clears the stored refresh token so it can no longer be redeemed.</summary>
    Task RevokeRefreshTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    /// <summary>Finds the user that owns the supplied refresh token, or <c>null</c>.</summary>
    Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
