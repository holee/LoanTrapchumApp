using Applications.Commons;
using Applications.Commons.Mappers;
using Applications.Extensions;
using Applications.Features.Journals.Dtos;
using Applications.Interfaces.Transactions;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.OtherNames;
using Dapper;
using Domains.Models.Journals;
using Domains.Models.Transaction;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructures.Implements;

public sealed class JournalRepository(DataContext context, ILogger<JournalRepository> logger, IDbContextFactory<DataContext> dbContextFactory,
    IDapperConnectionFactory dapperConnectionFactory)
    : IJournalService
{
    public async Task<ErrorOr<string>> UpdateJournalAsync(string? journalId, Domains.Models.Journals.JournalEntry journalEntry)
    {
        logger.LogInformation("Attempt to create Journal");
        try
        {
            var existingJournal = context.JournalEntries
                //.Include(x => x.LoanTransaction)
                .Include(j => j.JournalEntryLines)
                .FirstOrDefault(x => x.JournalEntryId == journalEntry.JournalEntryId);

            if (existingJournal is null)
            {
                _ = context.Add(journalEntry).Entity;
            }
            else
            {
                context.Entry(existingJournal)
                    .CurrentValues
                    .SetValues(journalEntry);

                foreach (var line in journalEntry.JournalEntryLines!)
                {
                    var existingJounralLine = existingJournal.JournalEntryLines!
                        .Where(x => x.JournalLineId == line.JournalLineId)
                        .FirstOrDefault();
                    if (existingJounralLine is null || existingJounralLine.JournalLineId == 0)
                    {
                        existingJournal.JournalEntryLines!.Add(line!);
                    }
                    else
                    {
                        context.Entry(existingJounralLine).CurrentValues.SetValues(line);
                    }
                }

                foreach (var item in existingJournal.JournalEntryLines!)
                {
                    if (!journalEntry.JournalEntryLines.Any(x => x.JournalLineId == item.JournalLineId))
                    {
                        context.Remove(item);
                    }
                }
            }

            await context.SaveChangesAsync();
            //if(existingJournal is null)
            //{
            //    //insertedEntity!.JournalLineCount = journalEntry.JournalEntryLines!.Count;
            //    insertedEntity!.TotalCr = journalEntry.JournalEntryLines!.Sum(x => x.Credit);
            //    insertedEntity!.TotalDr = journalEntry.JournalEntryLines!.Sum(x => (x.Debit));
            //}
            //else
            //{
            //    //existingJournal!.JournalLineCount = journalEntry.JournalEntryLines!.Count;
            //    existingJournal.TotalCr = journalEntry.JournalEntryLines!.Sum(x => x.Credit);
            //    existingJournal.TotalDr = journalEntry.JournalEntryLines!.Sum(x => (x.Debit));
            //}        
            //await context.SaveChangesAsync();

            // context.Database.CommitTransaction();

            return "New Journal Entry was Added.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<string>> UpdateJournalAsync(LoanTransaction tranx, CancellationToken cancellation = default)
    {
        logger.LogInformation("Attempt to create Journal");
        try
        {
            await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellation);

            var existingJournal = await dataContext.LoanTransaction
                                        .Include(x => x.JournalEntry)
                                        .ThenInclude(j => j.JournalEntryLines)
                                        .FirstOrDefaultAsync(x => x.TransactionsId == tranx.TransactionsId, cancellation);

            if (existingJournal is null)
            {
                return Error.NotFound(description: Messages.DataFound);
            }

            dataContext.Entry(existingJournal).CurrentValues.SetValues(tranx);

            // Update JournalEntry
            if (tranx.JournalEntry is not null && existingJournal.JournalEntry is not null)
            {
                dataContext.Entry(existingJournal.JournalEntry).CurrentValues.SetValues(tranx.JournalEntry);

                // Update JournalEntryLines
                foreach (var line in tranx.JournalEntry.JournalEntryLines ?? Enumerable.Empty<JournalEntryLine>())
                {
                    var existingLine = existingJournal?.JournalEntry?.JournalEntryLines?
                        .FirstOrDefault(x => x.JournalLineId == line.JournalLineId);

                    if (existingLine is null || line.JournalLineId == 0)
                    {
                        existingJournal?.JournalEntry.JournalEntryLines!.Add(line);
                    }
                    else
                    {
                        dataContext.Entry(existingLine).CurrentValues.SetValues(line);
                    }
                }

                // Remove lines not present in updated model
                var linesToRemove = existingJournal?.JournalEntry?.JournalEntryLines?
                    .Where(existing => !tranx.JournalEntry.JournalEntryLines!
                        .Any(updated => updated.JournalLineId == existing.JournalLineId))
                    .ToList();

                foreach (var removeLine in linesToRemove ?? [])
                {
                    dataContext.Remove(removeLine);
                }
            }

            await dataContext.SaveChangesAsync(cancellation);

            return "Journal entry successfully updated.";

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Unexpected(description: ex.Message);
        }
    }


    public async Task<ErrorOr<LoanTransaction>> CreateJournalAsync(LoanTransaction model)
    {
        logger.LogInformation("Attempt to create Journal");
        try
        {
            await using var dataContext = await dbContextFactory.CreateDbContextAsync();
            var insertedEntity = dataContext.Add(model).Entity;
            await dataContext.SaveChangesAsync();
            return insertedEntity;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Unexpected(description: ex.Message);
        }
    }


    public async Task<ErrorOr<JournalEntry>> CreateJournalAsync(JournalEntry model)
    {
        logger.LogInformation(@"=======================Attempt to create Journal=========================");

        await using var dataContext = await dbContextFactory.CreateDbContextAsync();

        try
        {
            var addedEntity = dataContext.JournalEntries.Add(model).Entity;

            await dataContext.SaveChangesAsync();

            return model;
        }
        catch (Exception ex)
        {
            logger.LogError("Error Server: " + ex, ex.Message);
            return Error.Unexpected(description: $"Error Server: {ex.Message}");
        }
    }


    public async Task<ErrorOr<string>> CreateJournalAsync(LoanTransaction model, CancellationToken cancellation = default)
    {
        logger.LogInformation(@"=======================Attempt to create Journal=========================");

        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellation);

        try
        {

            dataContext.LoanTransaction.Add(model);

            await dataContext.SaveChangesAsync(cancellation);

            return Messages.DataCreated;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError("Error Server: " + ex, ex.Message);
            return Error.Unexpected(description: $"Error Server: {ex.InnerException?.Message}");
        }
    }


    public async Task<ErrorOr<JournalEntry?>> CreateAsync(LoanTransaction model, CancellationToken cancellation = default)
    {
        logger.LogInformation("Attempt to create Journal");

        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellation);

        try
        {
            var addedModel = dataContext.LoanTransaction.Add(model).Entity;

            await dataContext.SaveChangesAsync(cancellation);

            return addedModel.JournalEntry;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError("Error Server: " + ex, ex.Message);
            return Error.Unexpected(description: $"Error Server: {ex.InnerException?.Message}");
        }
    }



    //
    public async Task<ErrorOr<string>> UpdateJournalAsync(string? id, LoanTransaction jTrx)
    {
        try
        {
            var trx = await context.LoanTransaction.Include(x => x.JournalEntry)
                .ThenInclude(x => x!.JournalEntryLines)
                .Where(x => x.TransactionsId == id)
                .FirstOrDefaultAsync();
            if (trx is null) return Error.NotFound(description: Messages.DataNotFound);

            await context.Database.BeginTransactionAsync();
            trx.TransactionTypeName = jTrx.TransactionTypeName;
            var lines = trx.JournalEntry?.JournalEntryLines;
            context.JournalEntryLines.RemoveRange(lines!);
            await context.SaveChangesAsync();
            int count = 1;
            jTrx.JournalEntry?.JournalEntryLines?.ForEach(x =>
            {
                x.LineSeq = count;
                count++;
            });
            trx.JournalEntry!.TotalDr = jTrx.JournalEntry?.JournalEntryLines?.Sum(x => (-x.Debit));
            trx.JournalEntry!.TotalCr = jTrx.JournalEntry?.JournalEntryLines?.Sum(x => x.Credit);
            trx.JournalEntry!.JournalLineCount = count - 1;
            trx.JournalEntry.JournalEntryLines = jTrx.JournalEntry!.JournalEntryLines;

            context.Update(trx);

            await context.SaveChangesAsync();

            await context.Database.CommitTransactionAsync();

            return Messages.DataCreated;
        }
        catch (Exception ex)
        {
            await context.Database.RollbackTransactionAsync();
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<ErrorOr<string>> DeleteJournalAsync(string? id)
    {
        try
        {
            await context.Database.BeginTransactionAsync();

            var journalEntry = await context.JournalEntries.FirstOrDefaultAsync(x => x.JournalEntryId == id);

            if (journalEntry is null)
            {
                return Error.NotFound(description: Messages.DataNotFound);
            }

            context.JournalEntries.Remove(journalEntry);

            await context.SaveChangesAsync();

            await context.Database.CommitTransactionAsync();

            return Messages.DataDeleted;
        }
        catch (Exception ex)
        {
            await context.Database.RollbackTransactionAsync();
            return Error.Unexpected(description: ex.Message);
        }
    }

    public async Task<Pager<JournalEntryVm>> GetJournalsAsync(int pageIndex, int pageSize,
        string sortColumn, bool sortDescending = false, string? q = null)
    {
        await using var dataContext = await dbContextFactory.CreateDbContextAsync();
        IQueryable<JournalEntryVm> queries = dataContext.VJournals
            .WhereIf(!string.IsNullOrWhiteSpace(q), x => EF.Functions.Like(x.ReferrenceNum!, $"%{q}%") ||
                                                         EF.Functions.Like(x.JournalType!, $"%{q}%") ||
                                                         EF.Functions.Like(x.TransactionTypeName!, $"%{q}%") ||
                                                         EF.Functions.Like(x.TransactionDate!.ToString(), $"%{q}%"))
            .OrderByDescendingIf(sortDescending, x => EF.Property<object>(x, sortColumn));
        var pageCount = await queries.CountAsync();
        var pageItems = queries.Skip(pageIndex).Take(pageSize).ToList();

        return new Pager<JournalEntryVm>
        {
            Count = pageCount,
            Items = pageItems
        };
    }

    public async Task<Pager<JournalEntryVm>> GetJournalsAsync(int? branchId, QueryParam param)
    {
        await using var dataContext = await dbContextFactory.CreateDbContextAsync();
        IQueryable<JournalEntryVm> queries = dataContext.VJournals
            .Where(x => x.BranchId == branchId)
            .WhereIf(!string.IsNullOrWhiteSpace(param.Filter), x =>
                EF.Functions.Like(x.ReferrenceNum!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.JournalType!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.TransactionTypeName!, $"%{param.Filter}%") ||
                EF.Functions.Like(x.TransactionDate!.ToString(), $"%{param.Filter}%"))
            .OrderByDescending(x => x.ModiFiedDT);
        if (param.SortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn));
        }

        var pageCount = await queries.CountAsync();

        var pageItems = await queries.Skip(param.Page).Take(param.Take)
            .AsNoTracking()
            .ToListAsync();

        return new Pager<JournalEntryVm>
        {
            Count = pageCount,
            Items = pageItems
        };
    }

    public async Task<IEnumerable<Domains.Models.Journals.JournalEntry>> GetJournalsAsync(string? journalId, string? q = null)
    {
        var queries = context.JournalEntries
                             .Include(x => x.JournalEntryLines)
                             .WhereIf(!string.IsNullOrEmpty(journalId), x => x.JournalEntryId == journalId);

        if (!string.IsNullOrEmpty(q))
        {
            var like = "%" + q + "%";

            queries = queries.Where(x => EF.Functions.Like(x.ReferrenceNum!, like) ||
                                         EF.Functions.Like(x.JournalType!, like) ||
                                         EF.Functions.Like(x.JournalEntryId, like));
        }

        return await queries.AsNoTracking().ToListAsync();
    }


    public async Task<PageMetaData<JournalEntryDto>> GetJournalsAsync(string? journalId, QueryParam param, CancellationToken cancellation = default)
    {

        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.Set<JournalEntryDto>()
                             .WhereIf(!string.IsNullOrEmpty(journalId), x => x.JournalEntryId == journalId);

        queries = ApplyFilter(param, queries);

        if (!string.IsNullOrEmpty(param.SortColumn))
        {
            var sortDescending = param.SortDescending;

            queries = sortDescending
                ? queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn))
                : queries.OrderBy(x => EF.Property<object>(x, param.SortColumn));
        }

        return await queries.ToPagedAsync(param.Page, param.Take, cancellation);
    }

    public async Task<ErrorOr<JournalEntryVm>> GetJournalByIdAsync(string? journalId, bool include)
    {
        if (journalId.IsNotNullOrEmpty())
        {
            return Error.NotFound(description: $"There is no journal with this id:{journalId}.");
        }

        using var db = dapperConnectionFactory.CreateConnection();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var parameters = new DynamicParameters();
        parameters.Add("@JounrnalEntryId", journalId);
        parameters.Add("@Include", include);
        if (!include)
        {
            var existingJournal =
                await db.QueryFirstOrDefaultAsync("spGetJournalEntries",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

            return existingJournal is null
                ? Error.NotFound(description: "Data Not Found.")
                : (ErrorOr<JournalEntryVm>)existingJournal;
        }

        var lookup = new Dictionary<string, JournalEntryVm>();
        var items = await db.QueryAsync<JournalEntryVm, JournalEntryLineVm, JournalEntryVm>("spGetJournalEntries",
            (journal, line) =>
            {
                if (lookup.TryGetValue(journal.JournalEntryId!, out var journalEntryVm))
                {
                    journal = journalEntryVm;
                }
                else
                {
                    lookup.Add(journal.JournalEntryId!, journalEntryVm!);
                }

                journal.JournalEntryLines?.Add(line);
                return journal;
            }, splitOn: "JournalLineId",
            param: parameters,
            commandType: CommandType.StoredProcedure);

        var item2 = items.FirstOrDefault(x => x.JournalEntryId == journalId);

        return item2 is null
            ? Error.NotFound(description: "Data Not Found.")
            : (ErrorOr<JournalEntryVm>)item2;
    }

    public async Task<Domains.Models.Journals.JournalEntry?> GetJournalByIdAsync(string? id)
    {
        var journal = context.JournalEntries
            .Where(x => x.JournalEntryId == id)
            .Include(a => a.JournalEntryLines);
        return await journal.AsNoTracking().FirstOrDefaultAsync();
    }


    public async Task<Domains.Models.Journals.JournalEntry?> GetJournalByIdAsync(string? journalEntryId, CancellationToken cancellation = default)
    {
        var foundItem = await context.JournalEntries
                                .Include(a => a.JournalEntryLines)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.JournalEntryId == journalEntryId, cancellation);
        return foundItem;
    }


    public async Task<Applications.Features.Journals.Dtos.JournalEntryDto?> GetJournalDetailByIdAsync(string? journalEntryId, CancellationToken cancellation = default)
    {
        using var db = await dapperConnectionFactory.CreateConnectionAsync(cancellation);

        var parameters = new DynamicParameters();

        parameters.Add("@JournalEntryId", journalEntryId); // ✅ fixed typo

        var journalDictionary = new Dictionary<string, Applications.Features.Journals.Dtos.JournalEntryDto>();

        var result = await db.QueryAsync(
            "spGetJournalEntryDetailsById", (Applications.Features.Journals.Dtos.JournalEntryDto journal, JournalEntryLineDto line) =>
            {
                if (!journalDictionary.TryGetValue(journal.JournalEntryId!, out var existingJournal))
                {
                    existingJournal = journal;

                    existingJournal.JournalEntryLines = new List<JournalEntryLineDto>();

                    journalDictionary.Add(existingJournal.JournalEntryId!, existingJournal);
                }

                if (line != null)
                {
                    existingJournal.JournalEntryLines.Add(line);
                }

                return existingJournal;
            },
            param: parameters,
            splitOn: "JournalLineId",
            commandType: CommandType.StoredProcedure);

        return journalDictionary.Values.FirstOrDefault();
    }


    public async Task<IEnumerable<JournalType>> GetJournalTypesAsync(string? expression = null)
    {
        IQueryable<JournalType> journalTypes = context.JournalTypes;
        if (!string.IsNullOrEmpty(expression))
        {
            journalTypes = journalTypes.Where(x =>
                x.Code != null && x.Code.ToLower().StartsWith(expression)
                || x.Description != null && x.Description.ToLower().StartsWith(expression));
        }

        return await journalTypes.AsNoTracking().ToListAsync() ?? Enumerable.Empty<JournalType>();
    }

    public async Task<Pager<JournalTypeDto>> GetJournalTypesAsync(int? journalTypeId, QueryParam param, CancellationToken cancellation = default)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = context.JournalTypes
                                  .WhereIf(journalTypeId.HasValue && journalTypeId > 0, x => x.Id == journalTypeId);
        queries = ApplyFilters(param, queries);

        queries = ApplySort(param, queries);

        var counts = await queries.CountAsync(cancellation);

        if (param.Take > 0)
        {
            queries = queries.Skip(param.Page)
                             .Take(param.Take);
        }

        var items = await queries.OrderByDescending(x => x.Id)
                        .AsNoTracking()
                        .Select(x => x.ToDto())
                       .ToListAsync(cancellation);

        return PagerExtensions.Create(items, counts);
    }


    public async Task<IEnumerable<VNameList>> GetNameLists(long? id = 0, int takeRows = 0, string? q = null)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var queries = context.VNameLists.AsNoTracking();

        if (!string.IsNullOrEmpty(q))
        {
            var filter = "%" + q + "%";

            queries = queries.Where(x => EF.Functions.Like(x.FullName, $"%{q}%") ||
                                         EF.Functions.Like(x.AltName, $"%{q}%"));
        }

        if (takeRows > 0)
        {
            queries = queries.Take(takeRows).OrderByDescending(x => x.FullName);
        }

        return await queries.AsNoTracking()
                            .ProjectToType<VNameList>()
                            .ToListAsync();
    }


    public async Task<IEnumerable<T>> GetNameLists<T>(string? nameId, QueryParam param) where T : class, new()
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        IQueryable<NameListVm> queries = context.Database
            .SqlQuery<NameListVm>($@"SELECT * FROM {nameof(VNameList)}")
            .WhereIf(!string.IsNullOrEmpty(param.Filter), x => EF.Functions.Like(x.FirstName!, $"%{param.Filter}%") ||
                                                               EF.Functions.Like(x.LastName!, $"%{param.Filter}%") ||
                                                               EF.Functions.Like(x.Name!, $"%{param.Filter}%") ||
                                                               EF.Functions.Like(x.AltName!, $"%{param.Filter}%"))
            .WhereIf(!string.IsNullOrEmpty(nameId), x => x.Id.Contains(nameId!))
            .OrderByDescendingIf(param.SortDescending, x => EF.Property<object>(x, param.SortColumn));

        if (param.Take > 0)
        {
            queries = queries.Take(param.Take).OrderByDescending(x => x.FullName);
        }

        var items = await queries
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();

        return items;
    }



    private static IQueryable<Applications.Features.Journals.Dtos.JournalEntryDto> ApplyFilter(QueryParam param, IQueryable<Applications.Features.Journals.Dtos.JournalEntryDto> queries)
    {
        var filter = param.Filter?.Trim().ToLower();

        if (string.IsNullOrEmpty(filter)) return queries;

        var like = "%" + filter + "%";

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.StartWiths(like, AllowJournalEntryColumns.ToArray());
        }
        else if (AllowJournalEntryColumns.Contains(param.FilterColumn))
        {
            queries = queries.StartWiths(like, param.FilterColumn);
        }

        return queries;
    }


    private static readonly HashSet<string> AllowJournalTypeColumns = new HashSet<string> {
        nameof(JournalType.Code),
        nameof(JournalType.Description)
    };

    private static readonly HashSet<string> AllowJournalEntryColumns = new HashSet<string> {
        nameof(JournalEntryDto.ReferrenceNum),
        nameof(JournalEntryDto.JournalType),
        nameof(JournalEntryDto.JournalEntryId),
        nameof(JournalEntryDto.JournalCode),
        nameof(JournalEntryDto.JournalDate),
        nameof(JournalEntryDto.BranchName),
        nameof(JournalEntryDto.TotalCr),
        nameof(JournalEntryDto.TotalDr),
    };

    private static IQueryable<JournalType> ApplySort(QueryParam param, IQueryable<JournalType> queries)
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
                      queries.OrderByDescending(x => x.Code)
                                .ThenByDescending(x => x.Description) :
                      queries.OrderBy(x => x.Code)
                             .ThenBy(x => x.Description);
        }

        return queries;
    }

    private static IQueryable<JournalType> ApplyFilters(QueryParam param, IQueryable<JournalType> queries)
    {
        var filter = param.Filter?.Trim();

        if (string.IsNullOrEmpty(filter))
        {
            return queries;
        }

        if (string.IsNullOrEmpty(param.FilterColumn))
        {
            queries = queries.StartWiths(filter, AllowJournalTypeColumns.ToArray());
        }
        else
        {
            if (AllowJournalTypeColumns.Contains(param.FilterColumn))
            {
                queries = queries.StartWiths(filter, param.FilterColumn);
            }
        }

        return queries;
    }

}

//public async Task<JournalEntry?> FindJournal(Expression<Func<JournalEntry, bool>> expression, params String[] includes)
//{
//    IQueryable<JournalEntry> journalEntries = context.JournalEntries;
//    if (includes.Length > 0)
//    {

//        foreach (var includeProp in includes)
//        {
//            journalEntries = journalEntries.Include(includeProp);
//        }
//    }

//    return await journalEntries.AsNoTracking().FirstOrDefaultAsync(expression);
//}


//public async Task<bool> CreateJournal(JournalEntry journalEntry)
//{

//    try
//    {
//        context.Database.BeginTransaction();

//        context.JournalEntries.Add(journalEntry);

//        await context.SaveChangesAsync();

//        var updateJournalEntryLines = new List<JournalEntryLine>();
//        int x = 1;
//        foreach (var item in journalEntry.JournalEntryLines ?? Enumerable.Empty<JournalEntryLine>())
//        {
//            item.LineSeq = x;
//            item.TranRefId = journalEntry.TranRefId;
//            updateJournalEntryLines.Add(item);
//            x++;
//        }

//        context.JournalEntryLines.UpdateRange(updateJournalEntryLines);

//        await context.SaveChangesAsync();


//        journalEntry.JournalLineCount = updateJournalEntryLines.Count();
//        journalEntry.TotalDr = updateJournalEntryLines.Sum(x => x.Debit);
//        journalEntry.TotalCr = updateJournalEntryLines.Sum(x => x.Credit);

//        context.JournalEntries.Entry(journalEntry).State = EntityState.Modified;

//        await context.SaveChangesAsync();

//        context.Database.CommitTransaction();
//        return true;

//    }
//    catch (Exception)
//    {
//        context.Database.RollbackTransaction();
//        throw;
//    }

//}

//public async Task<bool> UpdateJournal(Guid id, JournalEntry journalEntry)
//{
//    try
//    {
//        context.Database.BeginTransaction();
//        //delete journal entry lines
//        context.JournalEntryLines.Where(x => x.JournalRefId == id).ExecuteDelete();

//        //update only journal netry
//        context.Entry(journalEntry).State = EntityState.Modified;
//        //context.Entry(journalEntry.LoanTransaction).State = EntityState.Unchanged;

//        await context.SaveChangesAsync();
//        //update journal entry line
//        var updateJournalEntryLines = new List<JournalEntryLine>();
//        int x = 1;
//        foreach (var item in journalEntry.JournalEntryLines ?? Enumerable.Empty<JournalEntryLine>())
//        {
//            item.LineSeq = x;
//            item.TranRefId = journalEntry.TranRefId;
//            updateJournalEntryLines.Add(item);
//            x++;
//        }
//        context.JournalEntryLines.AddRange(updateJournalEntryLines);
//        await context.SaveChangesAsync();


//        journalEntry.JournalLineCount = updateJournalEntryLines.Count();
//        journalEntry.TotalDr = updateJournalEntryLines.Sum(x => x.Debit);
//        journalEntry.TotalCr = updateJournalEntryLines.Sum(x => x.Credit);
//        //context.JournalEntries.Entry(journalEntry).Property(x => x.JournalLineCount).IsModified = true;
//        //context.JournalEntries.Entry(journalEntry).Property(x=>x.TotalDr).IsModified=true;
//        //context.JournalEntries.Entry(journalEntry).Property(x => x.TotalCr).IsModified = true;
//        await context.SaveChangesAsync();

//        context.Database.CommitTransaction();

//        return true;

//    }
//    catch (Exception)
//    {
//        context.Database.RollbackTransaction();
//        throw;
//    }
//}

//public async Task<IEnumerable<JournalEntryVM>> FindAllJournalEntries()
//{
//    //IQueryable<JournalEntryVM> journalEntries = context.JournalEntries.Include(x => x.LoanTransaction)
//    //                                                      .Include(x => x.Currency)
//    //                                                      .Select(x => new JournalEntryVM
//    //                                                      {
//    //                                                          JournalId = x.JournalId,
//    //                                                          Date = x.JournalDate,
//    //                                                          JournalType = x.JournalType,
//    //                                                          JournalDesc = x.JournalDesc,
//    //                                                          CurrencyCode = x.Currency.CurrencyCode,
//    //                                                          TransactionType = x.LoanTransaction.LoanTransactionType.Description,
//    //                                                          TotalCr = x.TotalCr,
//    //                                                          TotalDr = x.TotalDr,
//    //                                                          ReferrenceNum = x.ReferrenceNum,
//    //                                                          IsAdjustment = x.IsAdjustment
//    //                                                      });


//    //return await journalEntries.AsNoTracking().ToListAsync();

//    await Task.Delay(1000);

//    return new List<JournalEntryVM>();
//}

//public async Task<PaginatedList<JournalEntryVM>> GetsJournalAsPagination(int pageNumber, int pageSize, string sortField, string sortOrder, string? expression = null)
//{
//    await Task.Delay(100);

//    return new PaginatedList<JournalEntryVM>();
//    //IQueryable<JournalEntryVM> journalEntries = context.JournalEntries
//    //                                                    .Include(x => x.LoanTransaction)
//    //                                                    .Include(x => x.Currency)
//    //                                                    .AsNoTracking()
//    //                                                    .Select(x => new JournalEntryVM
//    //                                                     {
//    //                                                         JournalId = x.JournalId,
//    //                                                         Date = x.JournalDate,
//    //                                                         JournalType = x.JournalType,
//    //                                                         JournalDesc = x.JournalDesc,
//    //                                                         CurrencyCode = x.Currency.CurrencyCode,
//    //                                                         TransactionType = x.LoanTransaction.LoanTransactionType.Description,
//    //                                                         TotalCr = x.TotalCr,
//    //                                                         TotalDr = x.TotalDr,
//    //                                                         ReferrenceNum = x.ReferrenceNum,
//    //                                                         IsAdjustment = x.IsAdjustment
//    //                                                     });


//    //if(!string.IsNullOrEmpty(expression))
//    //{
//    //    journalEntries = journalEntries.Where(x =>
//    //                           x.JournalType != null && x.JournalType.ToLower().StartsWith(expression) ||
//    //                           x.ReferrenceNum != null && x.ReferrenceNum.ToLower().StartsWith(expression) ||
//    //                           x.CurrencyCode != null && x.CurrencyCode.ToLower().StartsWith(expression)
//    //                       );
//    //}

//    //journalEntries = journalEntries.OrderByDynamic(sortField, sortOrder.ToUpper());
//    //return await PaginatedList<JournalEntryVM>.UpsertAsync(journalEntries, pageNumber, pageSize);

//}