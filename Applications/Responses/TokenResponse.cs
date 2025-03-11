namespace Domains.Response
{
    public record TokenResponse(string? TokenValue, string? RefreshToken,DateTime? RefreshTokenExpiryTime);
}
