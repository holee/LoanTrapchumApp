using Applications.Features.NbcBank.Dto;

namespace Applications.Features.NbcBank.Services;

public interface INbcExchangeRateService
{
    Task<NbcExchangeRateResponse> GetExchangeRatesAsync(string? language = "en", bool forceRefresh = false, CancellationToken cancellationToken = default);

    Task<CurrencyRate?> GetRateByCurrencyCodeAsync(string currencyCode, string? language = "en", CancellationToken cancellationToken = default);

    Task<ConversionResult> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount, string? language = "en", CancellationToken cancellationToken = default);
}
