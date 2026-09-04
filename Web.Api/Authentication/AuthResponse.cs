namespace Web.Api.Authentication;

/// <summary>
/// Login/refresh payload returned by <c>AuthController</c>.
/// A dedicated API contract is used instead of <c>UsersResponse</c> because that type marks
/// <c>RefreshToken</c> and <c>RefreshTokenExpiration</c> with <c>[JsonIgnore]</c>, which would
/// strip them from the response body.
/// </summary>
public sealed record AuthResponse
{
    public string Message { get; init; } = string.Empty;
    public bool IsAuthenticated { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? Role { get; init; }
    public IReadOnlyList<string> Roles { get; init; } = [];
    public int? BranchId { get; init; }
    public string Token { get; init; } = string.Empty;
    public DateTime TokenExpiration { get; init; }
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; init; }
}
