using Applications.Requests.Lists.Currency;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Currencies;

namespace Applications.Interfaces.Transactions
{
    public interface ISpotRateService
    {
        Task<IEnumerable<VCurrenciesList>> GetCurrenciesForSetSpotRate(bool includeBaseCurrency = true);
        Task<CurrencyDtoSpotRateCreate> GetSpotRateByDate(int currecnyRefId, DateTime date);
        Task<PaginatedList<CurrencyDtoSpotRate>> GetSpotRates(int currencyId, QueryParam param);
        Task<Result<string>> SetSpotRate(int currencyRefId, CurrencySpotRate rate);
        Task<Result<string>> SetSpotRates(List<CurrencySpotRate> rates);
    }
}
