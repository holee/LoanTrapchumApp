using Applications.Features.Currencies.Responses;

namespace Web.UI.Services;

public sealed class CurrencyServices
{
    public Task<VCurrenciesList?> GetExchangedCurrencies(int? currencyId)
    {
        return Task.FromResult<VCurrenciesList?>(null);
    }
}
