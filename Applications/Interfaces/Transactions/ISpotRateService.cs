namespace Applications.Interfaces.Transactions;

public interface ISpotRateService
{
    Task<PageMetaData<VCurrenciesList>> GetCurrenciesForSetSpotRate(bool includeBaseCurrency, QueryParam param, CancellationToken cancellation = default);
    Task<CurrencyDtoSpotRateCreate> GetSpotRateByDate(int? currecnyRefId, DateTime date, CancellationToken cancellation = default);
    Task<PageMetaData<CurrencyDtoSpotRate>> GetSpotRates(int? currencyId, QueryParam param, CancellationToken cancellation = default);
    Task<CurrencyDtoSpotRate> SetSpotRate(int? currencyRefId, CurrencySpotRate rate, CancellationToken cancellation = default);
    Task<string> SetSpotRates(List<CurrencySpotRate> rates, CancellationToken cancellation = default);
}
