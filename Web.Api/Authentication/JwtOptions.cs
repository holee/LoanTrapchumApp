namespace Web.Api.Authentication;

/// <summary>
/// Strongly typed binding of the "Jwt" configuration section.
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    /// <summary>Token issuer ("iss").</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>Token audience ("aud").</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Symmetric signing key. Never commit a real value: supply it through
    /// user-secrets (<c>dotnet user-secrets set "Jwt:Key" "..."</c>) or the
    /// <c>Jwt__Key</c> environment variable.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Access token lifetime in minutes.</summary>
    public int AccessTokenMinutes { get; set; } = 60;

    /// <summary>Refresh token lifetime in days.</summary>
    public int RefreshTokenDays { get; set; } = 7;
}
