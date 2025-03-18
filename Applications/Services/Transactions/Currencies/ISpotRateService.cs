using Applications.Interfaces.Transactions;
using Applications.Requests.Lists.Currency;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Currencies;

namespace Applications.Services.Transactions.Currencies
{
    public sealed class SpotRateService : ISpotRateService
    {
        public Task<IEnumerable<VCurrenciesList>> GetCurrenciesForSetSpotRate(bool includeBaseCurrency = true)
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyDtoSpotRateCreate> GetSpotRateByDate(int currecnyRefId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<CurrencyDtoSpotRate>> GetSpotRates(int currencyId, QueryParam param)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> SetSpotRate(int currencyRefId, CurrencySpotRate rate)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> SetSpotRates(List<CurrencySpotRate> rates)
        {
            throw new NotImplementedException();
        }
    }
}
