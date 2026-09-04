using System.Net.Http.Json;
using Applications.Utils;

namespace Web.UI.Services;

public sealed class ClientMediator(HttpClient httpClient) : IMediator
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        try
        {
            var requestType = request.GetType().Name;
            var response = await httpClient.PostAsJsonAsync($"/api/mediator/{requestType}", request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);
                if (result is not null) return result;
            }
        }
        catch
        {
            // Ignore in development/offline
        }

        return Activator.CreateInstance<TResponse>()!;
    }
}
