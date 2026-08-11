using Applications.Extensions.mappers;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using Mapster;

namespace Applications.Services.Transactions.Currencies
{
    public interface ISpotRateServiceImpl
    {
        Task<IEnumerable<VCurrenciesList>> GetCurrenciesForSetSpotRate(bool includeBaseCurrency = true);
        Task<CurrencyDtoSpotRateCreate> GetSpotRateByDate(int currecnyRefId, DateTime date);
        Task<PaginatedList<CurrencyDtoSpotRate>> GetSpotRates(int currencyId, QueryParam param);
        Task<Result<string>> SetSpotRate(int currencyRefId, SpotRateDtoCreate rate);
        Task<Result<string>> SetSpotRates(List<SpotRateDtoCreate> rates);
    }
    public sealed class SpotRateService
    {
        //public async Task<IEnumerable<VCurrenciesList>> GetCurrenciesForSetSpotRate(bool includeBaseCurrency = true)
        //{
        //    //return await rateService.GetCurrenciesForSetSpotRate(includeBaseCurrency);
        //}

        //public async Task<CurrencyDtoSpotRateCreate> GetSpotRateByDate(int currecnyRefId, DateTime date)
        //{
        //    return await rateService.GetSpotRateByDate(currecnyRefId, date);
        //}

        //public async Task<PaginatedList<CurrencyDtoSpotRate>> GetSpotRates(int currencyId, QueryParam param)
        //{
        //    return await rateService.GetSpotRates(currencyId, param);
        //}

        //public async Task<Result<string>> SetSpotRate(int currencyRefId, SpotRateDtoCreate rate)
        //{

        //    var req = rate.Adapt<CurrencySpotRate>();

        //    return await rateService.SetSpotRate(currencyRefId, req);
        //}

        //public async Task<Result<string>> SetSpotRates(List<SpotRateDtoCreate> rates)
        //{

        //    var req = rates.MapList<CurrencySpotRate>();

        //    return await rateService.SetSpotRates(req);

        //}
    }
}
