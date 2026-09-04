namespace Application.Responses
{
    public record TokenResponse(string? TokenValue, string? RefreshToken,DateTime? RefreshTokenExpiryTime);
}
