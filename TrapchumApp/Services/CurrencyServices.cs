using Applications.Features.Currencies.Queries.GetCurrencyDetails;
using Applications.Features.Currencies.Responses;
using Applications.Utils;

namespace TrapchumApp.Services;

public sealed class CurrencyServices(IServiceScopeFactory serviceScope)
{
    public async Task<VCurrenciesList?> GetExchangedCurrencies(int? currencyId)
    {
        if (!currencyId.HasValue || currencyId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currencyId));
        }

        using var scope = serviceScope.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var query = new GetCurrencyDetailsByIdQuery(currencyId.Value);

        var rsp = await mediator.Send(query);

        if (rsp.IsError)
        {
            throw new InvalidOperationException($"Failed to get currency details for currency ID {currencyId}. Error: {rsp.GetMessage()}");
        }

        return rsp.Value;
    }
}