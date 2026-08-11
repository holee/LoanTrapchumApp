using Applications.Commons;
using Applications.Commons.ExceptionHandlers;
using Applications.Extensions;
using Applications.Features.Ledgers.Dto;
using Applications.Interfaces.Transactions;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Ledgers;
using Dapper;
using Domains.Models.Journals;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Infrastructures.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class LedgerRepository : ILedgerService
{
    private readonly ILogger<LedgerRepository> _logger;
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    private readonly IDapperConnectionFactory _connectionFactory;
    public LedgerRepository(ILogger<LedgerRepository> logger,
        IDbContextFactory<DataContext> dbContextFactory, IDapperConnectionFactory connectionFactory)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
        _connectionFactory = connectionFactory;
    }

    public async Task<ErrorOr<string>> CreateOrUpdateLedger(Ledger entity)
    {
        _logger.LogInformation("try to create new or update ledger.");
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            if (entity.LedgerId <= 0)
            {
                var oldLedger = context.Ledgers.Where(x => x.LedgerName!.Equals(entity.LedgerName!));

                if (await oldLedger.AnyAsync())
                {
                    return Error.Conflict(description: Messages.DataExist);
                }

                context.Add(entity);
            }
            else
            {
                var oldLedger = context.Ledgers.Where(x => x.LedgerName!.Equals(entity.LedgerName!) &&
                                                            x.LedgerId != entity.LedgerId);
                if (await oldLedger.AnyAsync())
                {
                    return Error.Conflict(description: Messages.DataExist);
                }

                context.Update(entity);
            }

            await context.SaveChangesAsync();
            return "new ledger was created or updated successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteLedger(int? id)
    {
        _logger.LogInformation("try to delete ledger.....");

        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var usedLedgers = context.JournalEntryLines.Where(x => x.LedgerRefId.Equals(id));

            if (await usedLedgers.AnyAsync())
            {
                return Error.Conflict(description: Messages.DataExist);
            }

            var parentLedger = context.Ledgers.Where(x => x.ParentRefId.Equals(id));

            if (await parentLedger.AnyAsync())
            {
                return Error.Conflict(description: "ledger already in used.");
            }

            var ledger = await context.Ledgers.FirstOrDefaultAsync(x => x.LedgerId == id);

            if (ledger is null)
            {
                return Error.NotFound(description: Messages.DataNotFound);
            }

            context.Ledgers.Remove(ledger);

            await context.SaveChangesAsync();

            return "Ledger was remove successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Unexpected(description: ex.Message);
        }
    }


    public async Task DeleteAsync(int? id, CancellationToken cancellation = default)
    {
        _logger.LogInformation("try to delete ledger.....");

        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

            var usedLedgers = context.JournalEntryLines.Where(x => x.LedgerRefId == id);

            if (await usedLedgers.AnyAsync(cancellation))
            {
                throw new ConflictException(Messages.DataExist);
            }

            var parentLedger = context.Ledgers.Where(x => x.ParentRefId == id);

            if (await parentLedger.AnyAsync(cancellation))
            {
                throw new ConflictException("ledger already in used.");
            }

            var ledger = await context.Ledgers.FirstOrDefaultAsync(x => x.LedgerId == id, cancellation);

            if (ledger is null)
            {
                throw new NotFoundException(Messages.DataNotFound);
            }

            context.Ledgers.Remove(ledger);

            await context.SaveChangesAsync(cancellation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            throw new UnexpectedException(ex.Message);
        }
    }


    public async Task<ErrorOr<Ledger>> GetLedgerByIdAsync(int? ledgerId)
    {
        _logger.LogInformation("Retrieving Ledgers......!");
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var item = await context.Ledgers
                                        .Where(x => x.LedgerId == ledgerId)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();

            return item is null
                ? Error.NotFound(description: Messages.DataNotFound)
                : item;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<IEnumerable<LedgerType>>> FindLedgerTypes(int? ledgerTypeId = 0, string? q = null)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        IQueryable<LedgerType> queries = context.LedgerTypes;

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

        return items;
    }


    public async Task<ErrorOr<LedgerType>> GetLedgerTypeById(int? ledgerTypeId)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();

        var item = await context.LedgerTypes
                                    .Where(x => x.LedgerTypeId == ledgerTypeId)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

        return item is null
            ? Error.NotFound(description: Messages.DataNotFound)
            : item;
    }


    public async Task<Pager<VLedger>> GetLedgersAsync(int? currencyId, QueryParam q, CancellationToken cancellation = default)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.VLedgers
                            .IgnoreFilterIf(q.IncludeInActive)
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


        var counts = await queries.CountAsync(cancellation);

        List<VLedger> items;

        if (q.Page < 0)
        {
            items = await queries
                .AsNoTracking()
                .ToListAsync(cancellation);
        }
        else
        {
            items = await queries.Skip(q.Page)
                                 .Take(q.Take)
                                 .AsNoTracking()
                                 .ToListAsync(cancellation);
        }

        return new Pager<VLedger>
        {
            Count = counts,
            Items = items,
        };
    }


    public async Task<ErrorOr<IEnumerable<VLedger>>> GetLedgersAsync(int? ledgerId = 0, int? currencyId = 1, string? q = null)
    {
        try
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            IQueryable<VLedger> ledgers = context.VLedgers
                .OrderBy(x => x.LedgerTypeId)
                .ThenBy(x => x.LedgerTypeCode)
                .ThenBy(x => x.CurrencyCode)
                .ThenBy(x => x.LedgerName);

            if (ledgerId > 0)
                ledgers = ledgers.Where(x => x.LedgerId == ledgerId);

            if (currencyId > 0)
                ledgers = ledgers.Where(x => x.CurrencyId == currencyId);

            if (!string.IsNullOrEmpty(q))
                ledgers = ledgers.Where(x => EF.Functions.Like(x.LedgerCode!, $"{q}%") ||
                                             EF.Functions.Like(x.LedgerTypeCode!, $"{q}%") ||
                                             EF.Functions.Like(x.LedgerName!, $"{q}%"));

            var items = await ledgers.AsNoTracking().ToListAsync();

            return items;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: ex.Message);
        }
    }


    public async Task<PageMetaData<VLedger>> GetLedgersAsync(int? currencyId,
        QueryParam query,
        CancellationToken cancellation = default,
        params string[] columns)
    {

        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.VLedgers
                        .WhereIf(currencyId > 0, x => x.CurrencyId == currencyId)
                        .OrderBy(x => x.LedgerTypeId)
                        .ThenBy(x => x.LedgerTypeCode)
                        .ThenBy(x => x.CurrencyCode)
                        .ThenBy(x => x.LedgerName)
                        .AsNoTracking()
                        .AsQueryable();


        string filter = query.Filter?.Trim();

        if (!string.IsNullOrEmpty(query.FilterColumn))
        {
            var safeColumn = typeof(VLedger).MapField(query.FilterColumn);

            queries = queries.StartWiths(filter, safeColumn);
        }
        else
        {
            string[] defaultColumns = { nameof(VLedger.LedgerCode), nameof(VLedger.LedgerTypeCode), nameof(VLedger.LedgerName) };

            queries = queries.StartWiths(filter, defaultColumns);


        }

        queries = queries.ProjectTo<VLedger>(columns);

        return await queries.ToPagedAsync(query.Page, query.Take, cancellation);

    }


    public async Task<PageMetaData<VLedger>> GetLedgersAsync(int? ledgerId, int? currencyId, QueryParam? param, CancellationToken cancellation = default, params string[] columns)
    {
        _logger.LogInformation("Retrieving ledgers with ledger ID {LedgerId}, currency ID {CurrencyId}, and filter {Filter}.", ledgerId, currencyId, param?.Filter);

        using var connection = await _connectionFactory.CreateConnectionAsync(cancellation);

        var sqlColumns = columns.ToSafeColumnList<VLedger>();

        var conditions = new List<string>();

        var parameters = new DynamicParameters();

        if (ledgerId > 0)
        {
            conditions.Add("LedgerId = @LedgerId");

            parameters.Add("LedgerId", ledgerId);
        }

        if (currencyId > 0)
        {
            conditions.Add("CurrencyId = @CurrencyId");

            parameters.Add("CurrencyId", currencyId);
        }

        if (!string.IsNullOrEmpty(param?.Filter))
        {
            conditions.Add("(LedgerCode LIKE @Filter OR LedgerTypeCode LIKE @Filter OR LedgerName LIKE @Filter)");

            parameters.Add("Filter", $"{param.Filter}%");
        }

        if (!param.IncludeInActive)
        {
            conditions.Add("(IsActive = CAST(1 AS BIT))");
        }
        var whereSql = conditions.Count > 0 ? "WHERE " + string.Join(" AND ", conditions) : string.Empty;

        var fetchAll = (param?.Take ?? 0) <= 0;

        var take = param?.Take > 0 ? param.Take : int.MaxValue;

        var offset = param?.Take > 0 ? param.Page * param.Take : 0;

        var pagingSql = fetchAll ? string.Empty : "OFFSET @Offset ROWS FETCH NEXT @Take ROWS ONLY";

        var sql = $"""
            SELECT {sqlColumns}
            FROM VLedgers
            {whereSql}
            ORDER BY LedgerTypeId, LedgerTypeCode, CurrencyCode, LedgerName
            {pagingSql};

            SELECT COUNT(*)
            FROM VLedgers
            {whereSql};
            """;

        if (!fetchAll)
        {
            parameters.Add("Offset", offset);
            parameters.Add("Take", take);
        }

        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var items = (await multi.ReadAsync<VLedger>()).ToList();

        var totalCount = await multi.ReadSingleAsync<int>();

        return new PageMetaData<VLedger>
        {
            Items = items,
            Page = param?.Page ?? 0,
            PageSize = fetchAll ? totalCount : (param?.Take > 0 ? param.Take : totalCount),
            TotalCount = totalCount
        };
    }

    public async Task<ErrorOr<string>> ChangeLedgerStatus(LedgerChangeStatus req)
    {
        _logger.LogInformation("try to update ledger....");
        try
        {

            await using var context = await _dbContextFactory.CreateDbContextAsync();


            var exists = await context.Ledgers.AnyAsync(x => x.LedgerId == req.LedgerId);

            if (!exists)
            {
                return Error.NotFound(description: "No data found");
            }

            await context.Ledgers.Where(x => x.LedgerId == req.LedgerId)
                .ExecuteUpdateAsync(prop => prop.SetProperty(p => p.IsActive, req.Status));

            return Messages.DataUpdated;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Unexpected(description: ex.Message);
        }
    }
}


//public async Task<Pager<VLedger>> GetLedgersAsync(int? pageIndex, int? pageSize,
//    string sortColumn,
//    bool sortDescending = false,
//    int currencyId = 0,
//    bool includeInactive = false,
//    string? q = null)
//{

//    await using var context = await _dbContextFactory.CreateDbContextAsync();

//    IQueryable<VLedger> queries = context.VLedgers
//                                .OrderBy(x => x.LedgerTypeId)
//                                .ThenBy(x => x.LedgerTypeCode)
//                                .ThenBy(x => x.CurrencyCode)
//                                .ThenBy(x => x.LedgerName);

//    if (includeInactive)
//    {
//        queries = queries.IgnoreQueryFilters();
//    }

//    if (currencyId > 0)
//    {
//        queries = queries.Where(x => x.CurrencyId == currencyId);
//    }

//    if (sortDescending)
//    {
//        queries = queries.OrderByDescending(o => EF.Property<object>(o, sortColumn));
//    }

//    if (!string.IsNullOrEmpty(q))
//    {
//        var filter = "%" + q + "%";

//        queries = queries.Where(x => EF.Functions.Like(x.LedgerCode, filter) ||
//                                     EF.Functions.Like(x.CurrencyCode, filter) ||
//                                     EF.Functions.Like(x.LedgerName, filter) ||
//                                     EF.Functions.Like(x.LedgerId.ToString(), filter));
//    }


//    var counts = await queries.CountAsync();
//    var items = await queries.Skip(pageIndex!.Value)
//        .Take(pageSize!.Value)
//        .AsNoTracking()
//        .ToListAsync();
//    return new Pager<VLedger>
//    {
//        Count = counts,
//        Items = items,
//    };
//}
