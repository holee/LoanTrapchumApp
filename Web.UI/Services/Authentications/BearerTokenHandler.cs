namespace Web.UI.Services.Authentications;

/// <summary>Attaches the stored JWT as a bearer token on outgoing API calls.</summary>
public sealed class BearerTokenHandler(TokenStore tokenStore) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await tokenStore.LoadAsync();

        var token = tokenStore.AccessToken;

        if (!string.IsNullOrWhiteSpace(token) && request.Headers.Authorization is null)
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
