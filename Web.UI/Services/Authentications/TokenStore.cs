using Microsoft.JSInterop;

namespace Web.UI.Services.Authentications;

/// <summary>Persists the access/refresh token pair in browser localStorage.</summary>
public sealed class TokenStore(IJSRuntime js)
{
    private const string AccessTokenKey = "lms.accessToken";
    private const string RefreshTokenKey = "lms.refreshToken";

    // Cached so the auth state provider can read the token without an async JS hop on every request.
    private string? _accessToken;
    private string? _refreshToken;
    private bool _loaded;

    public string? AccessToken => _accessToken;

    public string? RefreshToken => _refreshToken;

    public async ValueTask LoadAsync()
    {
        if (_loaded) return;

        _accessToken = await GetItemAsync(AccessTokenKey);
        _refreshToken = await GetItemAsync(RefreshTokenKey);
        _loaded = true;
    }

    public async Task SaveAsync(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
        _loaded = true;

        await js.InvokeVoidAsync("localStorage.setItem", AccessTokenKey, accessToken);
        await js.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, refreshToken);
    }

    public async Task ClearAsync()
    {
        _accessToken = null;
        _refreshToken = null;
        _loaded = true;

        await js.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
        await js.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
    }

    private async ValueTask<string?> GetItemAsync(string key)
    {
        try
        {
            var value = await js.InvokeAsync<string?>("localStorage.getItem", key);

            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
        catch (JSException)
        {
            return null;
        }
    }
}
