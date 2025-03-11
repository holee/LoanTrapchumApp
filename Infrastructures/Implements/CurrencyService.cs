using Applications.Requests.Lists.Currency;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Applications.Services;
using Applications.Services.Currencies;
using Domains.Models.Currencies;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public class CurrencyService : ICurrencyService
{
    private readonly ApplicationDbContext _context;
    public CurrencyService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Currency> GetCurrencyWithSpotRates(int id)
    {
        var currency = await _context.Currencies
                                    .Include(sp => sp.SpotRates)
                                    .Where(c => c.CurrencyId == id)
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
        IQueryable<VCurrenciesList> queries = _context.VCurrenciesList;

        if (!string.IsNullOrEmpty(param.Filter))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CurrencyCode, $"%{param.Filter}") ||
                                         EF.Functions.Like(x.CurrencyName, $"%{param.Filter}"));
        }

        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        queries = queries.Skip(param.PageIndex).Take(param.PageSize);

        var counts = await queries.CountAsync();

        return new Pager<VCurrenciesList>(await queries.AsNoTracking().ToListAsync(), counts);
    }


    public async Task<Result<VCurrenciesList>> GetCurrencyAsync(int currencyId)
    {
        IQueryable<VCurrenciesList> queries = _context.VCurrenciesList
                                                    .Where(c => c.CurrencyId == currencyId);
        if (queries.Any())
        {
            var result = await queries.AsNoTracking().FirstOrDefaultAsync();

            return Result.Success(result!);
        }

        return Result.Failure<VCurrenciesList>(ErrorResponse.NotFound("No Currency found."));
    }
    public async Task<IEnumerable<VCurrenciesList>> GetCurrencies(string? q)
    {
        IQueryable<VCurrenciesList> queries = _context.VCurrenciesList;

        if (!string.IsNullOrEmpty(q) || !string.IsNullOrWhiteSpace(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.CurrencyCode, $"%{q}") || EF.Functions.Like(x.CurrencyName, $"%{q}"));
        }
        queries = queries.OrderByDescending(x => x.AsOfDate);

        return await queries.AsNoTracking().ToListAsync();
    }

    public async Task<Result<IEnumerable<Currency>>> GetCurrencies(int currencyId,string? q=null)
    {
        IQueryable<Currency> queries = _context.Currencies;

        if (!string.IsNullOrEmpty(q))
        {
            queries = queries.Where(x => EF.Functions.Like(x.Description, $"%{q}%"));
        }

        if(currencyId > 0)
        {
            queries = queries.Where(x => x.CurrencyId==currencyId);
        }

        var currencies = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<Currency>>(currencies);
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
        IQueryable<VCurrencyNotes> queries = _context.VCurrencyNotes;

        if (param.IsIncludeInActive)
        {
            queries = queries.IgnoreQueryFilters();
        }

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
        queries = queries.Skip(param.PageIndex).Take(param.PageSize);

        var counts = await queries.CountAsync();

        var items = await queries.AsNoTracking().ToListAsync();

        var pager = new Pager<VCurrencyNotes>(items, counts);

        return Result.Success(pager);



    }
    public async Task<Result<Pager<VCurrencyNotes>>> GetCurrencyNotes(int currencyId, QueryParam param)
    {
        IQueryable<VCurrencyNotes> queries = _context.VCurrencyNotes
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

        queries = queries.Skip(param.PageIndex).Take(param.PageSize);

        var counts = await queries.CountAsync();

        var items = await queries.AsNoTracking().ToListAsync();

        var pager = new Pager<VCurrencyNotes>(items, counts);

        return Result.Success(pager);
    }
    public async Task<Result> SaveOrUpdate(Currency currency)
    {
        try
        {
            if (currency.CurrencyId == 0)
            {
                var cur = await CheckMoneyExist(currency.CurrencyCode);

                if (cur.Exist) return Result.Failure(ErrorResponse.Conflict("Cuurency Code You are added already exist."));

                _context.Currencies.Add(currency);
            }
            else
            {
                var cur = CheckMoneyExist(currency.CurrencyId, currency.CurrencyCode);

                if (cur) return Result.Failure(ErrorResponse.Conflict("You are enter duplicate currency code."));

                _context.Currencies.Update(currency);
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
            var existing = _context.CurrencyCounters.Where(x =>
                                                        x.CurrencyRefId == model.CurrencyRefId &&
                                                        x.CurrencyValue == model.CurrencyValue)
                                                    .Any();

            if (existing) return Result.Failure(ErrorResponse.Conflict("Currency Note Value Already exist."));

            _context.CurrencyCounters.Add(model);

            await _context.SaveChangesAsync();

            return Result.Success();

        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException?.Message}"));
        }

    }
    public async Task<Result> AddNote(int currencyId, CurrencyCounter model)
    {
        try
        {
            var currency = await _context.Currencies.Where(x => x.CurrencyId == currencyId).FirstOrDefaultAsync();

            if (currency is null) return Result.Failure(ErrorResponse.NotFound("No Currency were selected."));

            var existing = _context.CurrencyCounters.Where(x =>
                                                        x.CurrencyRefId == model.CurrencyRefId &&
                                                        x.CurrencyValue == model.CurrencyValue)
                                                    .Any();

            if (existing) return Result.Failure(ErrorResponse.Conflict("Currency Note Value Already exist."));


            currency.CurrencyCounters!.Add(model);

            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException?.Message}"));
        }

    }
    private async Task<(bool Exist, Currency Currency)> CheckMoneyExist(string code)
    {
        var currency = await _context.Currencies.Where(c => c.CurrencyCode.ToLower() == code.ToLower()).FirstOrDefaultAsync();
        if (currency is null)
        {
            return (false, default!);
        }
        return (true, currency);
    }
    private bool CheckMoneyExist(int id, string code)
    {
        return _context.Currencies.Where(c => c.CurrencyCode.ToLower() == code.ToLower() &&
                                  c.CurrencyId != id).Any();
    }


    public async Task<Result> ActivateOrDeactivateNote(int noteId, NoteActivateDto model)
    {
        try
        {
            var note = await _context.CurrencyCounters.FirstOrDefaultAsync(x => x.Id == noteId);
            if (note is null)
            {
                return Result.Failure(ErrorResponse.NotFound("There is no any Note in the list."));
            }

            note.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            return Result.Success();

        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorResponse.ServerError($"{ex.InnerException?.Message}"));
        }

    }

    public async Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrecyNotes(int branchId)
    {
        var queries = _context.Database
                             .SqlQuery<VCurrencyNotes>($"SELECT * FROM fGetCurrencyNotesByBranch({branchId})");
        if (!queries.Any())
        {
            return Result.Failure<IEnumerable<VCurrencyNotes>>(ErrorResponse.NotFound("No Data Found."));
        }

        var items = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VCurrencyNotes>>(items);

    }

    public async Task<Result<IEnumerable<VCurrencyNotes>>> GetCurrecyNotes(int currencyId, int branchId)
    {
        var currencyQueries = _context.Database.SqlQuery<VCurrencyNotes>($"SELECT * FROM fGetCurrencyNotes({currencyId},{branchId})");

        if (!currencyQueries.Any())
        {
            return Result.Failure<IEnumerable<VCurrencyNotes>>(ErrorResponse.NotFound("No Data was found"));
        }

        var currencyData = await currencyQueries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<VCurrencyNotes>>(currencyData);

    }


    public async Task<IEnumerable<VCurrencyNotes>> GetCurrecyNotesss(int branchRefId)
    {
        var currencyNotes = _context.Database.SqlQuery<VCurrencyNotes>($"EXEC sprGetCurrencyNotes @branchId={branchRefId}");

        return await currencyNotes.AsNoTracking().ToListAsync();

    }


}
