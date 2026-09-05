using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.UI.Services.Authentications;

/// <summary>Builds the Blazor authentication state from the JWT held in <see cref="TokenStore"/>.</summary>
public sealed class JwtAuthenticationStateProvider(TokenStore tokenStore) : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await tokenStore.LoadAsync();

        var token = tokenStore.AccessToken;

        if (string.IsNullOrWhiteSpace(token) || JwtClaimsParser.IsExpired(token, TimeSpan.Zero))
        {
            return Anonymous;
        }

        return new AuthenticationState(BuildPrincipal(token));
    }

    public void NotifyUserAuthentication(string accessToken)
    {
        var state = Task.FromResult(new AuthenticationState(BuildPrincipal(accessToken)));

        NotifyAuthenticationStateChanged(state);
    }

    public void NotifyUserLogout() => NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));

    private static ClaimsPrincipal BuildPrincipal(string accessToken)
    {
        var identity = new ClaimsIdentity(
            JwtClaimsParser.Parse(accessToken),
            authenticationType: "jwt",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        return new ClaimsPrincipal(identity);
    }
}
