using Applications.Features.Currencies;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class SpotRateService : ISpotRateService
{

    private readonly DataContext _context;

    public SpotRateService(DataContext context)
    {
        this._context = context;
    }
    public async Task<Result<string>> SetSpotRate(int currencyRefId, CurrencySpotRate rate)
    {
        try
        {
            if (await _context.VSpotRates.AnyAsync(x => x.CurrencyRefId == currencyRefId && x.TransactionDate == rate.TransactionDate && x.Used == true))
            {
                return Result.Failure<string>(ErrorResponse.Conflict("Cannot change this spot rate because it is used by other tranasction"));
            }
            if (!IsSpotAlreadySet(currencyRefId, rate.TransactionDate))
            {
                var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == currencyRefId);

                if (currency is null)
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("There is no any currency selected"));
                }
                currency.AddSpotRate(rate);
            }
            else
            {
                var spotRate = _context.SpotRates.Where(sp => sp.TransactionDate == rate.TransactionDate &&
                                                    sp.CurrencyRefId == currencyRefId &&
                                                    sp.EditSeq == rate.EditSeq).FirstOrDefault();

                if (spotRate is null)
                {
                    return Result.Failure<string>(ErrorResponse.Conflict("Cannot change spot rate due to missing or someone changed it."));
                }

                spotRate.SpotRate = rate.SpotRate;
                spotRate.SellingRate = rate.SellingRate;
                spotRate.BuyingRate = rate.BuyingRate;
            }
            await _context.SaveChangesAsync();
            return Result.Success<string>(new("Spot Rate was set"));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }

    }
    public async Task<Result<string>> SetSpotRates(List<CurrencySpotRate> rates)
    {
        try
        {

            foreach (var rate in rates)
            {
                var existingSpotRate = _context.SpotRates
                                                .Where(x => x.TransactionDate.Equals(rate.TransactionDate)
                                                       && x.CurrencyRefId.Equals(rate.CurrencyRefId))
                                                    .FirstOrDefault();
                if (existingSpotRate is not null)
                {
                    try
                    {
                        _context.SpotRates.Where(prop => prop.TransactionDate == rate.TransactionDate
                                                     && prop.CurrencyRefId == rate.CurrencyRefId)
                                            .ExecuteUpdate(prop => prop
                                                    .SetProperty(x => x.SpotRate, rate.SpotRate)
                                                    .SetProperty(x => x.BuyingRate, rate.BuyingRate)
                                                    .SetProperty(x => x.SellingRate, rate.SellingRate));
                    }
                    catch (Exception ex)
                    {
                        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
                    }
                }
                else
                {
                    try
                    {
                        _context.SpotRates.Add(rate);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                        return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
                    }

                }
            }
            return Result.Success<string>(new("Spot Rate was set"));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }

    }

    public async Task<CurrencyDtoSpotRateCreate> GetSpotRateByDate(int currecnyRefId, DateTime date)
    {
        var currency = await _context.SpotRates
                                    .Where(c => c.CurrencyRefId == currecnyRefId && c.TransactionDate == date)
                                    .Select(x => new CurrencyDtoSpotRateCreate
                                    {
                                        BuyingRate = x.BuyingRate,
                                        EditSeq = x.EditSeq,
                                        SellingRate = x.SellingRate,
                                        TransactionDate = x.TransactionDate,
                                        SpotRate = x.SpotRate

                                    }).FirstOrDefaultAsync();
        return currency ?? default!;
    }
    public async Task<PaginatedList<CurrencyDtoSpotRate>> GetSpotRates(int currencyId, QueryParam param)
    {
        IQueryable<CurrencyDtoSpotRate> queries = _context.SpotRates.Where(x => x.CurrencyRefId == currencyId)
                                                       .OrderByDescending(x => x.TransactionDate)
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
        return await PaginatedList<CurrencyDtoSpotRate>.CreateAsync(queries, param.Page, param.Take);
    }
    public async Task<IEnumerable<VCurrenciesList>> GetCurrenciesForSetSpotRate(bool includeBaseCurrency = false)
    {
        IQueryable<VCurrenciesList> currencyList = _context.VCurrenciesList.Where(x => x.IsActive == true);

        if (!includeBaseCurrency)
        {
            currencyList = currencyList.Where(x => x.IsBaseCurrency == false);
        }
        return await currencyList.AsNoTracking().ToListAsync();
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














    private bool IsSpotAlreadySet(int currencyId, DateTime? setDate)
    {
        return _context.SpotRates.Where(sp => sp.TransactionDate == setDate && sp.CurrencyRefId == currencyId).Any();
    }
}
