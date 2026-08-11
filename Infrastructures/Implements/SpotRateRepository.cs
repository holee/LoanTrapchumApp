using Applications.Commons;
using Applications.Commons.ExceptionHandlers;
using Applications.Features.Currencies;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using Infrastructures.DataAccess;
using Infrastructures.DataAccess.Factories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public sealed class SpotRateRepository : ISpotRateService
{

    private readonly IDbContextFactory<DataContext> _contextFactory;
    public SpotRateRepository(IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<CurrencyDtoSpotRate> SetSpotRate(int? currencyRefId, CurrencySpotRate rate, CancellationToken cancellation = default)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            if (await context.VSpotRates.AnyAsync(x => x.CurrencyRefId == currencyRefId && x.TransactionDate == rate.TransactionDate && x.Used == true, cancellation))
            {
                throw new ConflictException("មិនអាចផ្លាស់ប្តូរអត្រាប្តូរប្រាក់ភ្លាមៗនេះបានទេ ព្រោះវាកំពុងត្រូវបានប្រើប្រាស់ដោយប្រតិបត្តិការផ្សេងទៀត។");
            }

            if (!await IsSpotAlreadySet(context, currencyRefId, rate.TransactionDate, cancellation))
            {
                var currency = await context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == currencyRefId, cancellation);

                if (currency is null)
                {
                    throw new NotFoundException("មិនមានរូបិយប័ណ្ណណាមួយត្រូវបានជ្រើសរើសទេ។");
                }

                currency.AddSpotRate(rate);
            }
            else
            {
                var spotRate = await context.SpotRates.FirstOrDefaultAsync(sp => sp.TransactionDate == rate.TransactionDate &&
                                                    sp.CurrencyRefId == currencyRefId &&
                                                    sp.EditSeq == rate.EditSeq, cancellation);

                if (spotRate is null)
                {
                    throw new ConflictException("មិនអាចផ្លាស់ប្តូរអត្រាប្តូរប្រាក់ភ្លាមៗបានទេ ដោយសារវាបាត់ ឬមានអ្នកផ្សេងបានផ្លាស់ប្តូរវារួចហើយ។");
                }

                spotRate.SpotRate = rate.SpotRate;

                spotRate.SellingRate = rate.SellingRate;

                spotRate.BuyingRate = rate.BuyingRate;
            }

            await context.SaveChangesAsync(cancellation);

            return new CurrencyDtoSpotRate
            {
                SpotRateId = rate.SpotRateId,
                SpotRate = rate.SpotRate,
                BuyingRate = rate.BuyingRate,
                SellingRate = rate.SellingRate,
                CurrencyRefId = rate.CurrencyRefId,
                TransactionDate = rate.TransactionDate,
                EditSeq = rate.EditSeq
            };
        }
        catch (Exception ex) when (ex is not ConflictException and not NotFoundException)
        {
            throw new Exception($"មានបញ្ហាកើតឡើងខណៈពេលកំណត់អត្រាប្តូរប្រាក់ភ្លាមៗ: {ex.Message}", ex);
        }

    }


    public async Task<string> SetSpotRates(List<CurrencySpotRate> rates, CancellationToken cancellation = default)
    {
        try
        {
            if (rates.Count == 0)
            {

                throw new NotFoundException("មិនមានទិន្នន័យអត្រាប្តូរប្រាក់ភ្លាមៗសម្រាប់កំណត់ទេ");
            }

            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var uniqueRates = rates
                                .GroupBy(x => new { x.CurrencyRefId, x.TransactionDate })
                                .Select(x => x.Last())
                                .ToList();

            var currencyIds = uniqueRates.Select(x => x.CurrencyRefId).Distinct().ToList();

            var transactionDates = uniqueRates.Select(x => x.TransactionDate).Distinct().ToList();

            var existingRates = await context.SpotRates
                                            .Where(x => currencyIds.Contains(x.CurrencyRefId)
                                                    && transactionDates.Contains(x.TransactionDate))
                                            .ToListAsync(cancellation);

            var existingLookup = existingRates.ToDictionary(x => (x.CurrencyRefId, x.TransactionDate));

            var newRates = new List<CurrencySpotRate>();

            foreach (var rate in uniqueRates)
            {
                if (existingLookup.TryGetValue((rate.CurrencyRefId, rate.TransactionDate), out var existingSpotRate))
                {
                    existingSpotRate.SpotRate = rate.SpotRate;

                    existingSpotRate.BuyingRate = rate.BuyingRate;

                    existingSpotRate.SellingRate = rate.SellingRate;
                }
                else
                {
                    newRates.Add(rate);
                }
            }

            if (newRates.Count > 0)
            {
                await context.SpotRates.AddRangeAsync(newRates, cancellation);
            }

            await context.SaveChangesAsync(cancellation);

            return "អត្រាប្តូរប្រាក់ភ្លាមៗត្រូវបានកំណត់រួចរាល់";
        }
        catch (Exception ex)
        {
            throw new Exception($"មានបញ្ហាកើតឡើងខណៈពេលកំណត់អត្រាប្តូរប្រាក់ភ្លាមៗ: {ex.Message}", ex);
        }

    }



    public async Task<CurrencyDtoSpotRateCreate> GetSpotRateByDate(int? currecnyRefId, DateTime date, CancellationToken cancellation = default)
    {

        await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

        var currency = await context.SpotRates
                                    .AsNoTracking()
                                    .Where(c => c.CurrencyRefId == currecnyRefId && c.TransactionDate == date)
                                    .Select(x => new CurrencyDtoSpotRateCreate
                                    {
                                        BuyingRate = x.BuyingRate,
                                        EditSeq = x.EditSeq,
                                        SellingRate = x.SellingRate,
                                        TransactionDate = x.TransactionDate,
                                        SpotRate = x.SpotRate

                                    }).FirstOrDefaultAsync(cancellation);

        if (currency is null)
        {
            throw new NotFoundException("មិនមានអត្រាប្តូរប្រាក់ភ្លាមៗ (Spot Rate) សម្រាប់រូបិយប័ណ្ណនេះនៅថ្ងៃដែលបានជ្រើសរើសទេ។");
        }

        return currency;
    }





    public async Task<PageMetaData<CurrencyDtoSpotRate>> GetSpotRates(int? currencyId, QueryParam param, CancellationToken cancellation = default)
    {

        await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

        var queries = context.SpotRates
                                .AsNoTracking()
                                .Where(x => x.CurrencyRefId == currencyId)
                                .OrderByDescending(x => x.TransactionDate)
                                .ThenByDescending(x => x.SpotRateId)
                                .Select(x => new CurrencyDtoSpotRate
                                {
                                    SpotRateId = x.SpotRateId,
                                    SpotRate = x.SpotRate,
                                    BuyingRate = x.BuyingRate,
                                    SellingRate = x.SellingRate,
                                    CurrencyRefId = x.CurrencyRefId,
                                    TransactionDate = x.TransactionDate,
                                    EditSeq = x.EditSeq
                                });
        return await queries.ToPagedAsync(param.Page, param.Take, cancellation);
    }


    public async Task<PageMetaData<VCurrenciesList>> GetCurrenciesForSetSpotRate(bool includeBaseCurrency, QueryParam param, CancellationToken cancellation = default)
    {

        await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

        var queries = context
                        .VCurrenciesList
                        .AsNoTracking()
                        .Where(x => x.IsActive);

        if (!includeBaseCurrency)
        {
            queries = queries.Where(x => !x.IsBaseCurrency);
        }

        queries = queries.OrderBy(x => x.CurrencyCode).ThenBy(x => x.CurrencyId);

        return await queries.ToPagedAsync(param.Page, param.Take, cancellation);
    }

















    //var AsOfDate = rates.Select(x => x.TransactionDate).ToArray();
    //var currencyIds = rates.Select(x => x.CurrencyRefId).ToArray();
    //var checkSpotRates = context.SpotRates.Where(x => AsOfDate.Contains(x.TransactionDate) &&
    //                                            currencyIds.Contains(x.CurrencyRefId)).ToList();
    //if (checkSpotRates.Any())
    //{
    //    return new MsgResponse()
    //    {
    //        IsSuccess = false,
    //        Message = "Some Spot Rate Already Set."
    //    };
    //}

    //var currencies = context.Currencies.Where(x => currencyIds.Contains(x.CurrencyId));
    //if (!currencies.Any())
    //{
    //    return new MsgResponse()
    //    {
    //        IsSuccess = false,
    //        Message = "There is any currency selected."
    //    };
    //}


    //context.SpotRates.AddRange(rates);














    private static Task<bool> IsSpotAlreadySet(DataContext context, int? currencyId, DateTime? setDate, CancellationToken cancellation = default)
    {
        return context.SpotRates.AnyAsync(sp => sp.TransactionDate == setDate && sp.CurrencyRefId == currencyId, cancellation);
    }
}
