using System.Net.Http.Json;
using Applications.Features.Identity.Users;

namespace Web.UI.Services.Authentications;

/// <summary>Login/refresh payload returned by the API's <c>AuthController</c>.</summary>
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

public sealed record RefreshTokenRequest(string RefreshToken);

/// <summary>Calls the API auth endpoints and keeps <see cref="JwtAuthenticationStateProvider"/> in sync.</summary>
public sealed class AuthenticationClient(
    HttpClient httpClient,
    TokenStore tokenStore,
    JwtAuthenticationStateProvider stateProvider)
{
    public async Task<(bool Success, string Message)> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return (false, await ReadProblemAsync(response, "Invalid credentials.", cancellationToken));
            }

            var payload = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken);

            if (payload is null || string.IsNullOrWhiteSpace(payload.Token))
            {
                return (false, "The server returned an empty token.");
            }

            await tokenStore.SaveAsync(payload.Token, payload.RefreshToken);

            stateProvider.NotifyUserAuthentication(payload.Token);

            return (true, payload.Message);
        }
        catch (HttpRequestException ex)
        {
            return (false, $"Could not reach the authentication server. {ex.Message}");
        }
    }

    /// <summary>Exchanges the stored refresh token for a new access token.</summary>
    public async Task<bool> TryRefreshAsync(CancellationToken cancellationToken = default)
    {
        await tokenStore.LoadAsync();

        if (string.IsNullOrWhiteSpace(tokenStore.RefreshToken)) return false;

        try
        {
            var response = await httpClient.PostAsJsonAsync(
                "api/auth/refresh",
                new RefreshTokenRequest(tokenStore.RefreshToken),
                cancellationToken);

            if (!response.IsSuccessStatusCode) return false;

            var payload = await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken);

            if (payload is null || string.IsNullOrWhiteSpace(payload.Token)) return false;

            await tokenStore.SaveAsync(payload.Token, payload.RefreshToken);

            stateProvider.NotifyUserAuthentication(payload.Token);

            return true;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        await tokenStore.LoadAsync();

        var refreshToken = tokenStore.RefreshToken;

        await tokenStore.ClearAsync();

        stateProvider.NotifyUserLogout();

        if (string.IsNullOrWhiteSpace(refreshToken)) return;

        try
        {
            await httpClient.PostAsJsonAsync("api/auth/revoke", new { Token = refreshToken }, cancellationToken);
        }
        catch (HttpRequestException)
        {
            // The local session is already cleared; a stale server-side refresh token expires on its own.
        }
    }

    /// <summary>Reads the <c>{ error }</c> / <c>{ errors: [] }</c> envelope produced by <c>ApiControllerBase.HandleResult</c>.</summary>
    private static async Task<string> ReadProblemAsync(HttpResponseMessage response, string fallback, CancellationToken cancellationToken)
    {
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ErrorPayload>(cancellationToken);

            if (!string.IsNullOrWhiteSpace(problem?.Error)) return problem.Error;

            var descriptions = problem?.Errors?
                .Select(e => e.Description)
                .Where(description => !string.IsNullOrWhiteSpace(description))
                .ToArray();

            return descriptions is { Length: > 0 } ? string.Join(" ", descriptions) : fallback;
        }
        catch
        {
            return fallback;
        }
    }

    private sealed record ErrorPayload(string? Error, IReadOnlyList<ErrorDetail>? Errors);

    private sealed record ErrorDetail(string? Code, string? Description);
}
