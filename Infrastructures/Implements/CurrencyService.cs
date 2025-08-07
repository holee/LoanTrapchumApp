using Applications.Extensions;
using Applications.Interfaces.Currencies;
using Applications.Requests.Lists.Currency;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Dapper;
using Domains.Models.Currencies;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructures.Implements;

public class CurrencyService : ICurrencyService
{
    private readonly DataContext _context;
    private readonly IDbContextFactory<DataContext> _contextFactory;
    private readonly IDapperConnectionFactory _connectionFactory;
    public CurrencyService(DataContext context, IDbContextFactory<DataContext> contextFactory, IDapperConnectionFactory connectionFactory)
    {
        _context = context;
        _contextFactory = contextFactory;
        _connectionFactory = connectionFactory;
    }

    public async Task<Currency> GetCurrencyWithSpotRates(int? currencyId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var currency = await context.Currencies
            .Include(sp => sp.SpotRates)
            .Where(c => c.CurrencyId == currencyId)
            .FirstOrDefaultAsync();
        return currency ?? default!;
    }

    public async Task<Currency> GetCurrencyById(int id)
    {
        var currency = await _context.Currencies
            .Where(c => c.CurrencyId == id)
            .FirstOrDefaultAsync();
        return currency ?? default!;
    }

    public async Task<Pager<VCurrenciesList>> GetCurrencies(QueryParam param)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<VCurrenciesList> queries = context.VCurrenciesList;

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CurrencyCode, $"%{param.Filter}") ||
                                         EF.Functions.Like(x.CurrencyName, $"%{param.Filter}"));
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var counts = queries.Count();

        var items = await queries.Skip(param.Skip)
            .Take(param.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return new Pager<VCurrenciesList>(items, counts);
    }


    public async Task<Result<VCurrenciesList>> GetCurrencyAsync(int? currencyId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var item = await context.VCurrenciesList
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CurrencyId == currencyId);
        if (item is not null)
        {
            return Result.Success(item);
        }

        return Result.Failure<VCurrenciesList>(ErrorResponse.NotFound("No Currency found."));
    }

    public async Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? filter)
    {
        await using var _ctx = await _contextFactory.CreateDbContextAsync();

        var queries = await _ctx.Database
            .SqlQuery<VCurrenciesList>(@$"
                                                                               SELECT c.* FROM  VCurrenciesLists c WHERE c.IsActive = 1 
                                                                               ")
            .WhereIf(!string.IsNullOrEmpty(filter), x => EF.Functions.Like(x.CurrencyCode, $"%{filter}%") ||
                                                         EF.Functions.Like(x.CurrencyName, $"%{filter}%"))
            .AsNoTracking()
            .ToListAsync();
        return queries;
    }

    public async Task<Result<IEnumerable<Currency>>> GetCurrencies(int? currencyId, string? q = null)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<Currency> queries = context.Currencies;

        if (!string.IsNullOrEmpty(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.Description, $"%{q}%"));
        }

        if (currencyId > 0)
        {
            queries = queries.Where(x => x.CurrencyId == currencyId);
        }

        var currencies = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<Currency>>(currencies);
    }


    public async Task<Result<IEnumerable<T>>> GetCurrencies<T>(int? currencyId, string? q = null)
        where T : class, new()
    {
        
        await using var context = await _contextFactory.CreateDbContextAsync();

        var queries = await context.Currencies
            .WhereIf(currencyId > 0, x => x.CurrencyId == currencyId)
            .WhereIf(!string.IsNullOrEmpty(q), x => EF.Functions.Like(x.Description, $"%{q}%"))
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();
        return Result.Success<IEnumerable<T>>(queries);
    }

    public async Task<CurrencyCounter> GetCurrencyCounter(int id)
    {
        var currency = await _context.CurrencyCounters
            .Include(l => l.Currency)
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        return currency ?? default!;
    }

    public async Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(QueryParam param)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<VCurrencyNotes> queries = context.Database
            .SqlQuery<VCurrencyNotes>(@$"SELECT * FROM VCurrencyNotes")
            .WhereIf(!param.IsIncludeInActive, x => x.IsActive == true);

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(c => EF.Functions.Like(c.CurrencyName, $"%{param.Filter}%"));
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        if (!queries.Any())
        {
            return Result.Failure<Pager<VCurrencyNotes>>(ErrorResponse.NotFound("No Data Found"));
        }

        queries = queries.Skip(param.Skip).Take(param.PageSize);

        var counts = await queries.CountAsync();

        var items = await queries.AsNoTracking().ToListAsync();

        var pager = new Pager<VCurrencyNotes>(items, counts);

        return Result.Success(pager);
    }

    public async Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, QueryParam param)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        IQueryable<VCurrencyNotes> queries = context.VCurrencyNotes
            .Where(x => x.CurrencyRefId == currencyId);

        if (param.IsIncludeInActive)
        {
            queries = queries.IgnoreQueryFilters();
        }

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CurrencyName!, $"%{param.Filter}%"));
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        queries = queries.Skip(param.Skip).Take(param.PageSize);

        var counts = queries.Count();

        var items = await queries.AsNoTracking().ToListAsync();

        var pager = new Pager<VCurrencyNotes>(items, counts);

        return Result.Success(pager);
    }

    public async Task<Result> SaveOrUpdate(Currency currency)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();


            if (currency.CurrencyId == 0)
            {
                var cur = await CheckMoneyExist(currency.CurrencyCode);

                if (cur.Exist)
                    return Result.Failure(ErrorResponse.Conflict("Cuurency Code You are added already exist."));

                context.Currencies.Add(currency);
            }
            else
            {
                var cur = await CheckMoneyExist(currency.CurrencyId, currency.CurrencyCode);

                if (cur) return Result.Failure(ErrorResponse.Conflict("You are enter duplicate currency code."));

                context.Currencies.Update(currency);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException?.Message}"));
        }
    }

    public async Task<Result> AddNote(CurrencyCounter model)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var existing = context.CurrencyCounters.Where(x =>
                    x.CurrencyRefId == model.CurrencyRefId &&
                    x.CurrencyValue == model.CurrencyValue)
                .Any();

            if (existing) return Result.Failure(ErrorResponse.Conflict("Currency Note Value Already exist."));

            context.CurrencyCounters.Add(model);

            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException?.Message}"));
        }
    }

    public async Task<Result> AddNote(int? currencyId, CurrencyCounter model)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();


            var currency = await context.Currencies.Where(x => x.CurrencyId == currencyId).FirstOrDefaultAsync();

            if (currency is null) return Result.Failure(ErrorResponse.NotFound("No Currency were selected."));

            var existing = context.CurrencyCounters.Where(x =>
                    x.CurrencyRefId == model.CurrencyRefId &&
                    x.CurrencyValue == model.CurrencyValue)
                .Any();

            if (existing) return Result.Failure(ErrorResponse.Conflict("Currency Note Value Already exist."));


            currency.CurrencyCounters!.Add(model);

            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException?.Message}"));
        }
    }

    private async Task<(bool Exist, Currency Currency)> CheckMoneyExist(string code)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

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
        using var context = await _contextFactory.CreateDbContextAsync();

        return context.Currencies.Where(c => c.CurrencyCode.ToLower() == code.ToLower() &&
                                             c.CurrencyId != id).Any();
    }


    public async Task<Result> ActivateOrDeactivateNote(int? noteId, NoteActivateDto model)
    {
        try
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var note = await context.CurrencyCounters.FirstOrDefaultAsync(x => x.Id == noteId);

            if (note is null)
            {
                return Result.Failure(ErrorResponse.NotFound("There is no any Note in the list."));
            }

            note.IsActive = model.IsActive;

            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException?.Message}"));
        }
    }

    public async Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? branchId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var queries = context.Database.SqlQuery<VCurrencyNotes>($"SELECT * FROM fGetCurrencyNotesByBranch({branchId})");
        if (!queries.Any())
        {
            return Result.Failure<IEnumerable<VCurrencyNotes>>(ErrorResponse.NotFound("No Data Found."));
        }

        var items = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VCurrencyNotes>>(items);
    }

    public async Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrencyNotes(int? currencyId, int? branchId)
    {
        using var db=await _connectionFactory.CreateConnectionAsync();


        var queries =await db.QueryAsync<VCurrencyNotes>(ProcedureConstant.GetCurrencyNotes, 
                                        new { currencyId, branchId },
                                        commandType:CommandType.StoredProcedure);
        if (!queries.Any())
        {
            return Result.Failure<IEnumerable<VCurrencyNotes>>(ErrorResponse.NotFound("No Data was found"));
        }

        return Result.Success<IEnumerable<VCurrencyNotes>>(queries);
    }


    public async Task<IEnumerable<VCurrencyNotes>> GetCurrecyNotesss(int? branchRefId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var currencyNotes =
            context.Database.SqlQuery<VCurrencyNotes>($"EXEC sprGetCurrencyNotes @branchId={branchRefId}");

        return await currencyNotes.AsNoTracking().ToListAsync();
    }

    public async Task<Result<Currency>> GetCurrencyById(int? id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var currency = await context.Currencies
            .Where(x => x.CurrencyId == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (currency is null)
        {
            return Result.Failure<Currency>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        return Result.Success<Currency>(currency);
    }
}