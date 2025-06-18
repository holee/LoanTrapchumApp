using Applications.Extensions;
using Applications.Interfaces.Transactions;
using Applications.Requests.Journals;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Ledgers;
using Domains.Models.Journals;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public class LedgerService : ILedgerService
{
    private readonly DataContext _context;
    private readonly ILogger<LedgerService> _logger;
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    public LedgerService(DataContext context, ILogger<LedgerService> logger, IDbContextFactory<DataContext> dbContextFactory)
    {
        this._context = context;
        this._logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<string>> CreateOrUpdateLedger(Ledger entity)
    {
        _logger.LogInformation("try to create new or update ledger.");
        try
        {
            if (entity.LedgerId <= 0)
            {
                var oldLedger = _context.Ledgers.Where(x => x.LedgerName!.Equals(entity.LedgerName!));

                if (oldLedger.Any())
                {
                    return Result.Failure<string>(ErrorResponse.Conflict(Messages.DataExist));
                }

                _context.Add(entity);
            }
            else
            {
                var oldLedger = _context.Ledgers.Where(x => x.LedgerName!.Equals(entity.LedgerName!) &&
                                                            x.LedgerId != entity.LedgerId);
                if (oldLedger.Any())
                {
                    return Result.Failure<string>(ErrorResponse.Conflict(Messages.DataExist));
                }

                _context.Update(entity);
            }

            await _context.SaveChangesAsync();
            return Result.Success<string>(new("new ledger was created or updated successfully."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result<string>> DeleteLedger(int? id)
    {
        _logger.LogInformation("try to delete ledger.....");
        try
        {
            var usedLedgers = _context.JournalEntryLines.Where(x => x.LedgerRefId.Equals(id));

            if (usedLedgers.Any())
            {
                return Result.Failure<string>(ErrorResponse.Conflict(Messages.DataExist));
            }

            var parentLedger = _context.Ledgers.Where(x => x.ParentRefId.Equals(id));

            if (parentLedger.Any())
            {
                return Result.Failure<string>(ErrorResponse.ServerError("ledger already in used."));
            }

            var ledger = await _context.Ledgers.FirstOrDefaultAsync(x => x.LedgerId == id);

            if (ledger is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            _context.Ledgers.Remove(ledger);

            await _context.SaveChangesAsync();

            return Result.Success<string>(new("Ledger was remove successfully."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result<Ledger>> FindLedgerByIdAsync(int? ledgerId)
    {
        _logger.LogInformation("Retrieving Ledgers......!");
        try
        {
            IQueryable<Ledger> ledgers = _context.Ledgers
                .Where(x => x.LedgerId == ledgerId);

            if (!ledgers.Any())
            {
                return Result.Failure<Ledger>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            var item = await ledgers.AsNoTracking().FirstOrDefaultAsync();

            return Result.Success<Ledger>(item!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<Ledger>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result<IEnumerable<LedgerType>>> FindLedgerTypes(int? ledgerTypeId = 0, string? q = null)
    {
        IQueryable<LedgerType> queries = _context.LedgerTypes;

        if (ledgerTypeId > 0)
        {
            queries = queries.Where(x => x.LedgerTypeId == ledgerTypeId);
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            var filter = "%" + q + "%";
            queries = queries.Where(x => EF.Functions.Like(x.LedgerTypeCode, filter) ||
                                         EF.Functions.Like(x.LedgerTypeId!.ToString(), filter) ||
                                         EF.Functions.Like(x.CrOrDr, filter));
        }

        var items = await queries.OrderBy(x => x.LedgerTypeCode)
            .AsNoTracking()
            .ToListAsync();

        return Result.Success<IEnumerable<LedgerType>>(items);
    }


    public async Task<Result<LedgerType>> FindLedgerTypeById(int? ledgerTypeId)
    {
        IQueryable<LedgerType> queries = _context.LedgerTypes.Where(x => x.LedgerTypeId == ledgerTypeId);

        if (!queries.Any())
        {
            return Result.Failure<LedgerType>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        var item = await _context.LedgerTypes.AsNoTracking().FirstAsync();

        return Result.Success(item);
    }

    public async Task<Pager<VLedger>> GetLedgersAsync(int? pageIndex, int? pageSize,
        string sortColumn,
        bool sortDescending = false,
        int currencyId = 0,
        bool includeInactive = false,
        string? q = null)
    {
        IQueryable<VLedger> queries = _context.VLedgers
            .OrderBy(x => x.LedgerTypeId)
            .ThenBy(x => x.LedgerTypeCode)
            .ThenBy(x => x.CurrencyCode)
            .ThenBy(x => x.LedgerName);

        if (includeInactive)
        {
            queries = queries.IgnoreQueryFilters();
        }

        if (currencyId > 0)
        {
            queries = queries.Where(x => x.CurrencyId == currencyId);
        }

        if (sortDescending)
        {
            queries = queries.OrderByDescending(o => EF.Property<object>(o, sortColumn));
        }

        if (!string.IsNullOrEmpty(q))
        {
            var filter = "%" + q + "%";

            queries = queries.Where(x => EF.Functions.Like(x.LedgerCode, filter) ||
                                         EF.Functions.Like(x.CurrencyCode, filter) ||
                                         EF.Functions.Like(x.LedgerName, filter) ||
                                         EF.Functions.Like(x.LedgerId.ToString(), filter));
        }


        var counts = await queries.CountAsync();
        var items = await queries.Skip(pageIndex!.Value)
            .Take(pageSize!.Value)
            .AsNoTracking()
            .ToListAsync();
        return new Pager<VLedger>
        {
            Count = counts,
            Items = items,
        };
    }

    public async Task<Pager<VLedger>> GetLedgersAsync(int? currencyId, QueryParam q, CancellationToken cancellation = default)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.VLedgers
                            .IgnoreFilterIf(q.IsIncludeInActive)
                            .WhereIf(currencyId > 0, x => x.CurrencyId == currencyId)
                            .OrderBy(x => x.LedgerTypeId)
                            .ThenBy(x => x.LedgerTypeCode)
                            .ThenBy(x => x.CurrencyCode)
                            .ThenBy(x => x.LedgerName)
                            .AsNoTracking();

        if (q.SortDescending)
        {
            queries = queries.OrderByDescending(o => EF.Property<object>(o, q.SortColumn));
        }

        if (!string.IsNullOrEmpty(q.Filter))
        {
            var filter = "%" + q.Filter + "%";

            queries = queries.Where(x => EF.Functions.Like(x.LedgerCode, filter) ||
                                         EF.Functions.Like(x.CurrencyCode, filter) ||
                                         EF.Functions.Like(x.LedgerName, filter) ||
                                         EF.Functions.Like(x.LedgerId.ToString(), filter));
        }


        var counts = await queries.CountAsync();
        var items = await queries.Skip(q.Skip)
                                    .Take(q.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
        return new Pager<VLedger>
        {
            Count = counts,
            Items = items,
        };
    }


    public async Task<Result<IEnumerable<VLedger>>> GetLedgersAsync(int? ledgerId = 0, int? currencyId = 1, string? q = null)
    {
        try
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync();

            IQueryable<VLedger> ledgers = _context.VLedgers
                .OrderBy(x => x.LedgerTypeId)
                .ThenBy(x => x.LedgerTypeCode)
                .ThenBy(x => x.CurrencyCode)
                .ThenBy(x => x.LedgerName);

            if (ledgerId > 0)
            {
                ledgers = ledgers.Where(x => x.LedgerId == ledgerId);
            }

            if (currencyId > 0)
            {
                ledgers = ledgers.Where(x => x.CurrencyId == currencyId);
            }

            if (!string.IsNullOrEmpty(q))
            {
                ledgers = ledgers.Where(x => EF.Functions.Like(x.LedgerCode!, $"%{q}%") ||
                                             EF.Functions.Like(x.LedgerTypeCode!, $"%{q}%") ||
                                             EF.Functions.Like(x.LedgerName!, $"%{q}%"));
            }

            if (!ledgers.Any())
            {
                return Result.Failure<IEnumerable<VLedger>>(ErrorResponse.ServerError(Messages.DataNotFound));
            }

            var items = await ledgers.AsNoTracking().ToListAsync();

            return Result.Success<IEnumerable<VLedger>>(items);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<VLedger>>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    public async Task<Result<string>> ChangeLedgerStatus(LedgerChangeStatus req)
    {
        _logger.LogInformation("try to update ledger....");
        try
        {
            var result = _context.Ledgers.Where(x => x.LedgerId == req.LedgerId);

            if (!result.Any())
            {
                return Result.Failure<string>(ErrorResponse.NotFound("Do data found"));
            }

            await _context.Ledgers.Where(x => x.LedgerId == req.LedgerId)
                .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.IsActive, req.Status));

            return Result.Success(Messages.DataUpdated);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, ex.Message);

            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }
}