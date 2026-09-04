using Applications.Commons;
using Applications.Commons.Extensions.QueryableExtensions;
using Applications.Extensions;
using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Currencies;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Domains.Models.Currencies;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Repository.Extensions;
using Infrastructures.Utils;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructures.Repository;

public sealed class CurrencyRepository(DataContext contextFactory) : ICurrencyService
{

    public async Task<ErrorOr<Currency>> GetCurrencyByIdAsync(int? currencyId, CancellationToken cancellationToken = default, params Expression<Func<Currency, object>>[] includes)
    {
        var context = contextFactory;

        IQueryable<Currency> query = context.Currencies.AsNoTracking();

        if (includes is not null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        var currency = await query.SingleOrDefaultAsync(x => x.CurrencyId == currencyId, cancellationToken);

        return currency is null
            ? Error.NotFound(Messages.DataNotFound)
            : currency;
    }


    public async Task<ErrorOr<CurrencyDto>> GetCurrencyById(int? currencyId, CancellationToken cancellationToken = default, params Expression<Func<Currency, object>>[] includes)
    {
        if (currencyId is null or <= 0)
        {
            return Error.Validation(description: "Currency Id must be greater than zero.");
        }

        var context = contextFactory;

        var queries = context.Currencies.AsNoTracking()
                                            .Where(x => x.CurrencyId == currencyId);

        if (includes is not null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                queries = queries.Include(include);
            }
        }

        var currency = await queries
                            .AsSplitQuery()
                            .Select(x => new CurrencyDto
                            {
                                CurrencyId = x.CurrencyId,
                                CurrencyCode = x.CurrencyCode,
                                Description = x.Description,
                                ExchangeRate = x.ExchangeRate,
                                SpotRate = x.SpotRate,
                                CurrencyType = x.CurrencyType,
                                Limited = x.Limited,
                                IsBaseCurrency = x.IsBaseCurrency ?? false,
                                IsActive = x.IsActive,
                                CurrencySpotRates = x.SpotRates!.Select(s => new CurrencySpotRateDto
                                {
                                    SpotRateId = s.SpotRateId,
                                    BuyingRate = s.BuyingRate,
                                    AvgRate = s.AvgRate,
                                    Description = s.Description,
                                    TransactionDate = s.TransactionDate,
                                    SellingRate = s.SellingRate,
                                    SpotRate = s.SpotRate
                                }).ToList(),
                                CurrencyCounters = x.CurrencyCounters!.Select(c => new CurrencyCounterDto
                                {
                                    Id = c.Id,
                                    CurrencyValue = c.CurrencyValue,
                                    Description = c.Description,
                                    Quantity = c.Quantity,
                                    IsActive = c.IsActive
                                }).ToList()
                            })
                        .SingleOrDefaultAsync(cancellationToken);

        return currency is null
            ? Error.NotFound(Messages.DataNotFound)
            : currency;
    }


    public async Task<Pager<VCurrenciesList>> GetCurrencies(QueryParam param, CancellationToken cancellation = default)
    {
        var context = contextFactory;

        var queries = context.VCurrenciesList.AsQueryable();

        queries = ApplyFilters(param, queries);

        queries = ApplySort(param, queries);

        var counts = queries.Count();

        if (param.Page > 0)
        {
            queries = queries.Skip(param.Page).Take(param.Take);
        }

        var items = await queries.AsNoTracking()
                                .ToListAsync(cancellation);

        return new Pager<VCurrenciesList>(items, counts);
    }


    public async Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? filter)
    {
        var _ctx = contextFactory;

        var queries = await _ctx.Database
            .SqlQuery<VCurrenciesList>(@$"SELECT c.* FROM  VCurrenciesLists c WHERE c.IsActive = 1")
            .WhereIf(!string.IsNullOrEmpty(filter), x => EF.Functions.Like(x.CurrencyCode, $"%{filter}%") ||
                                                         EF.Functions.Like(x.CurrencyName, $"%{filter}%"))
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }


    public async Task<ErrorOr<VCurrenciesList>> SaveOrUpdate(Currency currency, CancellationToken cancellation = default)
    {
        try
        {
            var context = contextFactory;

            if (currency.CurrencyId == 0)
            {
                var cur = await CheckMoneyExist(currency.CurrencyCode);

                if (cur.Exist)
                    return Error.Conflict("Cuurency Code You are added already exist.");

                context.Currencies.Add(currency);
            }
            else
            {
                var cur = await CheckMoneyExist(currency.CurrencyId, currency.CurrencyCode);

                if (cur) return Error.Conflict("You are enter duplicate currency code.");

                context.Currencies.Update(currency);
            }

            var createdCurrency = await GetCurrencyDetailById(currency.CurrencyId, cancellation);

            return createdCurrency;
        }
        catch (Exception ex)
        {
            return Error.Unexpected("ServerError", ex.Message);
        }
    }

    public async Task<ErrorOr<VCurrencyCounter>> AddNote(CurrencyCounter model, CancellationToken cancellation = default)
    {
        try
        {
            var context = contextFactory;

            var existing = await context.CurrencyCounters
                        .AsNoTracking()
                        .AnyAsync(x =>
                    x.CurrencyRefId == model.CurrencyRefId &&
                    x.CurrencyValue == model.CurrencyValue, cancellation);

            if (existing) return Error.Conflict("Currency Note Value Already exist.");

            context.CurrencyCounters.Add(model);

            await context.SaveChangesAsync(cancellation);

            var createdNote = await GetCurrencyNote(model.Id, cancellation);

            return createdNote;
        }
        catch (Exception ex)
        {
            return Error.Unexpected("ServerError", ex.Message);
        }
    }

    public async Task<ErrorOr<VCurrencyCounter>> AddNote(int? currencyId, CurrencyCounter model, CancellationToken cancellation = default)
    {
        try
        {
            var context = contextFactory;

            var currency = await context.Currencies.Where(x => x.CurrencyId == currencyId).FirstOrDefaultAsync();

            if (currency is null) return ErrorOr.Error.NotFound("No Currency were selected.");

            var existing = context.CurrencyCounters.Where(x =>
                    x.CurrencyRefId == model.CurrencyRefId &&
                    x.CurrencyValue == model.CurrencyValue)
                .Any();

            if (existing) return Error.Conflict("Currency Note Value Already exist.");

            currency.CurrencyCounters!.Add(model);

            await context.SaveChangesAsync();

            var createdNote = await GetCurrencyNote(model.Id, cancellation);

            return createdNote;
        }
        catch (Exception ex)
        {
            return Error.Unexpected("ServerError", ex.Message);
        }
    }

    public async Task<ErrorOr<string>> ActivateOrDeactivateNote(int? noteId, NoteActivateDto model, CancellationToken cancellation = default)
    {
        try
        {
            var context = contextFactory;

            var note = await context.CurrencyCounters.FirstOrDefaultAsync(x => x.Id == noteId);

            if (note is null)
            {
                return Error.NotFound("There is no any Note in the list.");
            }

            note.IsActive = model.IsActive;

            await context.SaveChangesAsync(cancellation);

            return Messages.DataUpdated;
        }
        catch (Exception ex)
        {
            return Error.Unexpected("ServerError", ex.Message);
        }
    }

    public async Task<Pager<VCurrencyCounter>> GetCurrencyCounter(int? branchId, int? currencyId, CancellationToken ca = default)
    {
        var db = contextFactory;

        var queries = db.Set<VCurrencyCounter>()
                        .Where(x => x.BranchId == branchId)
                        .WhereIf(currencyId.HasValue && currencyId > 0, x => x.CurrencyId == currencyId);

        var counts = await queries.CountAsync(ca);
        var items = await queries
                        .AsNoTracking()
                        .Select(x => new VCurrencyCounter
                        {
                            Id = x.Id,
                            CurrencyId = x.CurrencyId,
                            CurrencyCode = x.CurrencyCode,
                            CurrencyName = x.CurrencyName,
                            CurrencyValue = x.CurrencyValue,
                            Description = x.Description,
                            Quantity = x.Quantity,
                            IsActive = x.IsActive
                        })
                        .OrderBy(x => x.CurrencyCode)
                        .ToListAsync(ca);
        return PagerExtensions.Create(items, counts);

    }

    public async Task<Pager<VCurrencyNotes>> GetCurrenciesNotes(int? branchId, int? currencyId, QueryParam param, CancellationToken ca = default)
    {
        var db = contextFactory;

        var queries = db.Set<VCurrencyNotes>()
                         .Where(x => x.BranchId == branchId)
                         .WhereIf(currencyId.HasValue && currencyId > 0, x => x.CurrencyId == currencyId);

        queries = ApplyFilters(currencyId, param, queries);

        queries = ApplySort(param, queries);

        var counts = await queries.CountAsync(ca);

        if (param.Take > 0)
        {
            queries = queries.Skip(param.Page).Take(param.Take);
        }

        var items = await queries.AsNoTracking().ToListAsync(ca);

        return PagerExtensions.Create(items, counts);
    }

    public async Task<ErrorOr<VCurrencyCounter>> GetCurrencyNote(int? noteId, CancellationToken cancellation = default)
    {
        var context = contextFactory;

        var queries = await context.VCurrencyNotes
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == noteId, cancellation);

        if (queries is null)
        {
            return Error.NotFound(Messages.DataNotFound);
        }

        return queries;

    }



    public async Task<PageMetaData<CurrencyDto>> GetCurrenciesAsync(int? currencyId, QueryParam param,
        CancellationToken cancellation = default,
        params string[] columns)
    {
        var context = contextFactory;

        var baseQueries = context.Currencies
                            .WhereIf(currencyId.HasValue && currencyId > 0, x => x.CurrencyId == currencyId)
                            .AsQueryable();

        baseQueries = ApplyFilter(param, baseQueries);

        baseQueries = ApplySort(param, baseQueries);

        var newQueries = baseQueries.MapToType<Currency, CurrencyDto>(columns);

        return await newQueries.ToPagedAsync(param.Page, param.Take, cancellation);
    }



    private static IQueryable<Currency> ApplySort(QueryParam param, IQueryable<Currency> baseQueries)
    {
        var sortDescending = param.SortDescending;

        if (!string.IsNullOrEmpty(param.FilterColumn))
        {

            baseQueries = sortDescending ?
                        baseQueries.OrderByDescending(x => EF.Property<object>(x, param.FilterColumn)) :
                        baseQueries.OrderBy(x => EF.Property<object>(x, param.FilterColumn));

        }
        else
        {
            baseQueries = sortDescending ?
                        baseQueries.OrderByDescending(c => c.CurrencyCode)
                                .ThenByDescending(c => c.Description) :
                        baseQueries.OrderBy(x => x.CurrencyCode)
                                .ThenBy(x => x.Description);
        }

        return baseQueries;
    }

    public async Task<ErrorOr<VCurrenciesList>> GetCurrencyDetailById(int? currencyId, CancellationToken cancellation = default)
    {
        var context = contextFactory;

        var currency = await context.VCurrenciesList
                              .AsNoTracking()
                              .FirstOrDefaultAsync(x => x.CurrencyId == currencyId, cancellation);
        if (currency is null) return Error.NotFound(Messages.DataNotFound);

        return currency;

    }


    private static CurrencyDto ProjectCurrencyDto(Currency x)
    {
        return new CurrencyDto
        {
            CurrencyId = x.CurrencyId,
            CurrencyCode = x.CurrencyCode,
            Description = x.Description,
            ExchangeRate = x.ExchangeRate,
            SpotRate = x.SpotRate,
            CurrencyType = x.CurrencyType,
            Limited = x.Limited,
            IsBaseCurrency = x.IsBaseCurrency ?? false,
            IsActive = x.IsActive
        };
    }

    private static IQueryable<Currency> ApplyFilter(QueryParam param, IQueryable<Currency> baseQueries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrWhiteSpace(filter)) return baseQueries;

        if (string.IsNullOrWhiteSpace(param.FilterColumn))
        {
            string[] props = { nameof(Currency.CurrencyCode), nameof(Currency.Description) };

            baseQueries = baseQueries.StartWiths(filter, props);
        }
        else
        {

            baseQueries = baseQueries.StartWith(filter, param.FilterColumn);
        }

        return baseQueries;
    }

    private static IQueryable<VCurrenciesList> ApplyFilters(QueryParam param, IQueryable<VCurrenciesList> queries)
    {

        var filterParam = param.Filter?.Trim();

        if (!string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.Where(x => EF.Functions.Like(param.FilterColumn, $"{param.Filter}%"));
        }
        else
        {

            string[] props = ["CurrencyCode", "CurrencyName"];

            queries = queries.StartWiths(filterParam, props);
        }

        return queries;
    }

    private static IQueryable<VCurrenciesList> ApplySort(QueryParam param, IQueryable<VCurrenciesList> queries)
    {
        var sortDescending = param.SortDescending;

        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            queries = sortDescending ?
                        queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn)) :
                        queries.OrderBy(x => EF.Property<object>(x, param.SortColumn));

            return queries;
        }

        queries = sortDescending ?
                    queries.OrderByDescending(c => c.CurrencyName)
                            .ThenByDescending(c => c.CurrencyId)
                            .ThenByDescending(c => c.CurrencyCode) :
                    queries.OrderBy(x => x.CurrencyName)
                            .ThenBy(x => x.CurrencyId)
                            .ThenBy(x => x.CurrencyCode);
        return queries;
    }

    private static IQueryable<CurrencyNoteViewDto> ApplyFilter(int? currencyId, QueryParam param, IQueryable<CurrencyNoteViewDto> queries)
    {
        if (currencyId.HasValue && currencyId > 0)
        {
            queries = queries.Where(x => x.CurrencyRefId == currencyId);
        }

        if (!string.IsNullOrEmpty(param.Filter) && string.IsNullOrEmpty(param.FilterColumn))
        {
            var filter = $"%{param.Filter.Trim()}%";

            string[] props = { "CurrencyName", "CurrencyCode" };

            queries = queries.StartWiths(filter, props);
        }

        if (!string.IsNullOrEmpty(param.Filter) && !string.IsNullOrEmpty(param.FilterColumn))
        {
            var filter = $"%{param.Filter.Trim()}%";

            string[] props = { "CurrencyName", "CurrencyCode" };

            queries = queries.Where(x => EF.Functions.Like(param.SortColumn, filter));
        }

        return queries;
    }

    private static IQueryable<CurrencyNoteViewDto> ApplySort(QueryParam param, IQueryable<CurrencyNoteViewDto> queries)
    {
        var sortDescending = param.SortDescending;

        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            queries = sortDescending ?
                        queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn)) :
                        queries.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }
        else
        {
            queries = sortDescending ?
                        queries.OrderByDescending(c => c.CurrencyName)
                                .ThenByDescending(c => c.CurrencyValue) :
                        queries.OrderBy(x => x.CurrencyName)
                                .ThenBy(x => x.CurrencyValue);
        }

        return queries;
    }

    private static IQueryable<VCurrencyCounter> ApplySort(QueryParam param, IQueryable<VCurrencyCounter> queries)
    {

        var sortDescending = param.SortDescending;


        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            queries = sortDescending ?
                        queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn)) :
                         queries.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }
        else
        {
            queries = sortDescending ?
                        queries.OrderByDescending(c => c.CurrencyName)
                                .ThenByDescending(c => c.CurrencyValue) :
                        queries.OrderBy(x => x.CurrencyName)
                                .ThenBy(x => x.CurrencyValue);
        }

        return queries;
    }

    private static IQueryable<VCurrencyCounter> ApplyFilter(QueryParam param, IQueryable<VCurrencyCounter> queries)
    {
        if (param.IncludeInActive)
        {
            queries = queries.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CurrencyName!, $"%{param.Filter}%"));
        }

        return queries;
    }

    private static IQueryable<VCurrencyNotes> ApplyFilters(int? currencyId, QueryParam param, IQueryable<VCurrencyNotes> queries)
    {
        var filter = param.Filter!.Trim();

        if (string.IsNullOrEmpty(filter)) return queries;

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, AllowProps.ToArray());
        }
        else
        {
            if (AllowProps.Contains(param.FilterColumn))
            {
                queries = queries.Contains(filter, param.FilterColumn);
            }
        }

        return queries;
    }

    private static IQueryable<VCurrencyNotes> ApplySort(QueryParam param, IQueryable<VCurrencyNotes> queries)
    {
        var sortDescending = param.SortDescending;

        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            queries = sortDescending ?
                    queries.OrderByDescending(x => param.SortColumn) :
                    queries.OrderBy(x => param.SortColumn);
        }
        else
        {
            queries = sortDescending ?
                    queries.OrderByDescending(x => x.CurrencyCode)
                                .ThenByDescending(x => x.CurrencyValue)
                                    .ThenByDescending(x => x.CurrencyName) :
                    queries.OrderBy(x => x.CurrencyCode)
                            .ThenBy(x => x.CurrencyValue)
                                .ThenBy(x => x.CurrencyName);
        }

        return queries;
    }

    private async Task<(bool Exist, Currency Currency)> CheckMoneyExist(string code)
    {
        var context = contextFactory;

        var currency = await context.Currencies.Where(c => c.CurrencyCode.ToLower() == code.ToLower())
            .FirstOrDefaultAsync();

        if (currency is null)
        {
            return (false, default!);
        }

        return (true, currency);
    }

    private async Task<bool> CheckMoneyExist(int id, string code)
    {
        var context = contextFactory;

        return context.Currencies.Where(c => c.CurrencyCode.ToLower() == code.ToLower() &&
                                             c.CurrencyId != id).Any();
    }


    private static readonly HashSet<string> AllowedFilterColumns = new HashSet<string>
    {
        nameof(Currency.CurrencyCode),
        nameof(Currency.Description)
    };


    private static readonly HashSet<string> AllowProps = new HashSet<string> {
                       nameof(VCurrencyNotes.CurrencyName),
                       nameof(VCurrencyNotes.CurrencyValue),
                       nameof(VCurrencyNotes.CurrencyCode),
                       nameof(VCurrencyNotes.Description),
                };

}



//public async Task<PageMetaData<CurrencyDto>> GetCurrenciesAsync(int? currencyId, QueryParam param, CancellationToken cancellation = default)
//{
//    var context = contextFactory;

//    var baseQueries = context.Currencies
//                        .WhereIf(currencyId.HasValue && currencyId > 0, x => x.CurrencyId == currencyId)
//                        .AsQueryable();

//    baseQueries = ApplyFilter(param, baseQueries);

//    baseQueries = ApplySort(param, baseQueries);

//    var counts = await baseQueries.CountAsync(cancellation);

//    if (param.Take > 0)
//    {
//        baseQueries = baseQueries.Skip(param.Page).Take(param.Take);
//    }

//    var items = await baseQueries
//                        .AsNoTracking()
//                        .Select(x => ProjectCurrencyDto(x))
//                        .ToListAsync(cancellation);

//    return PagerExtensions.Create(items, counts);

//}
//public async Task<ErrorOr<Currency>> GetCurrencyWithSpotRates(int? currencyId, CancellationToken cancellation = default)
//{
//    var context = contextFactory;

//    var currency = await context.Currencies
//                        .Include(sp => sp.SpotRates)
//                        .AsNoTracking()
//                        .FirstOrDefaultAsync(c => c.CurrencyId == currencyId, cancellation);

//    if (currency is null) return Error.NotFound(Messages.DataNotFound);
//    return currency;
//}


//public async Task<ErrorOr<Currency>> GetCurrencyById(int? id, CancellationToken cancellation = default)
//{
//    var context = contextFactory;

//    var currency = await context.Currencies
//                            .AsNoTracking()
//                            .FirstOrDefaultAsync(c => c.CurrencyId == id, cancellation);
//    if (currency is null) return Error.NotFound(Messages.DataNotFound);

//    return currency;
//}



//public async Task<Result<IEnumerable<Currency>>> GetCurrencies(int? currencyId, string? q = null)
//{
//    var context = contextFactory;

//    IQueryable<Currency> queries = context.Currencies;

//    if (!string.IsNullOrEmpty(q))
//    {
//        queries = queries.Where(x => EF.Functions.Like(x.Description, $"%{q}%"));
//    }

//    if (currencyId > 0)
//    {
//        queries = queries.Where(x => x.CurrencyId == currencyId);
//    }

//    var currencies = await queries.AsNoTracking().ToListAsync();

//    return Result.Success<IEnumerable<Currency>>(currencies);
//}

//public async Task<Result<Currency>> GetCurrencyById(int? currencyId, CancellationToken cancellation = default)
//{
//    var context = contextFactory;

//    var currency = await context.Currencies
//        .AsNoTracking()
//        .FirstOrDefaultAsync(x => x.CurrencyId == currencyId, cancellation);
//    if (currency is null)
//    {
//        return Result.Failure<Currency>(ErrorResponse.NotFound(Messages.DataNotFound));
//    }

//    return Result.Success<Currency>(currency);
//}


//public async Task<IEnumerable<VCurrencyCounter>> GetCurrecyNotesss(int? branchRefId)
//{
//    var context = contextFactory;

//    var currencyNotes =
//        context.Database.SqlQuery<VCurrencyCounter>($"EXEC sprGetCurrencyNotes @branchId={branchRefId}");

//    return await currencyNotes.AsNoTracking().ToListAsync();
//}

//public async Task<Result<IEnumerable<VCurrencyCounter>>> GetCurrencyNotes(int? branchId)
//{
//    var context = contextFactory;

//    var queries = context.Database.SqlQuery<VCurrencyCounter>($"SELECT * FROM fGetCurrencyNotesByBranch({branchId})");

//    if (!queries.Any())
//    {
//        return Result.Failure<IEnumerable<VCurrencyCounter>>(ErrorResponse.NotFound("No Data Found."));
//    }

//    var items = await queries.AsNoTracking().ToListAsync();

//    return Result.Success<IEnumerable<VCurrencyCounter>>(items);
//}

//public async Task<CurrencyCounter> GetCurrencyCounter(int id)
//{
//    var currency = await context.CurrencyCounters
//        .Include(l => l.Currency)
//        .Where(c => c.Id == id)
//        .FirstOrDefaultAsync();
//    return currency ?? default!;
//}
//public async Task<Result<VCurrenciesList>> GetCurrencyAsync(int? currencyId, CancellationToken cancellation = default)
//{
//    var context = contextFactory;

//    var item = await context.VCurrenciesList
//                            .AsNoTracking()
//                            .FirstOrDefaultAsync(c => c.CurrencyId == currencyId, cancellation);
//    if (item is not null)
//    {
//        return Result.Success(item);
//    }

//    return Result.Failure<VCurrenciesList>(ErrorResponse.NotFound("No Currency found."));
//}

//public async Task<Result<IEnumerable<T>>> GetCurrencies<T>(int? currencyId, string? q = null)
//    where T : class, new()
//{

//    var context = contextFactory;

//    var queries = await context.Currencies
//        .WhereIf(currencyId > 0, x => x.CurrencyId == currencyId)
//        .WhereIf(!string.IsNullOrEmpty(q), x => EF.Functions.Like(x.Description, $"%{q}%"))
//        .ProjectToType<T>()
//        .AsNoTracking()
//        .ToListAsync();
//    return Result.Success<IEnumerable<T>>(queries);
//}
//public async Task<Pager<CurrencyNoteViewDto>> GetCurrencyNotes(int? branchId, int? currencyId, QueryParam param, CancellationToken cancellation = default)
//{
//    var context = contextFactory;

//    var queries = context.Set<CurrencyNoteViewDto>()
//                         .WhereIf(branchId.HasValue && branchId > 0, x => x.BranchRefId == branchId);

//    queries = ApplyFilter(currencyId, param, queries);

//    queries = ApplySort(param, queries);

//    var counts = await queries.CountAsync(cancellation);

//    if (param.Skip > 0)
//    {
//        queries = queries.Skip(param.Skip).Take(param.PageSize);
//    }

//    var items = await queries
//                         .AsNoTracking()
//                        .ToListAsync(cancellation);

//    return new Pager<CurrencyNoteViewDto>(items, counts);
//}

//public async Task<Pager<VCurrencyCounter>> GetCurrencyCounter(int? currencyId, QueryParam param, CancellationToken cancellation = default)
//{
//    var context = contextFactory;

//    IQueryable<VCurrencyCounter> queries = context.VCurrencyNotes
//                                            .WhereIf(currencyId.HasValue && currencyId > 0, x => x.CurrencyId == currencyId);

//    queries = ApplyFilter(param, queries);

//    queries = ApplySort(param, queries);

//    var counts = queries.Count();

//    if (param.Page > 0)
//    {
//        queries = queries.Skip(param.Page).Take(param.Take);

//    }

//    var items = await queries.AsNoTracking().ToListAsync(cancellation);

//    var pager = new Pager<VCurrencyCounter>(items, counts);

//    return pager;
//}