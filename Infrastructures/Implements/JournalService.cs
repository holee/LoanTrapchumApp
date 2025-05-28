using System.Data;
using Applications.Extensions;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Applications.Responses.ViewModels.OtherNames;
using Dapper;
using Domains.Models.Journals;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public class JournalService(
    DataContext context,
    ILogger<JournalService> logger,
    IDbContextFactory<DataContext> dbContextFactory,
    IDapperConnectionFactory dapperConnectionFactory)
    : IJournalService
{
    private readonly IDapperConnectionFactory _dapperConnectionFactory = dapperConnectionFactory;

    public async Task<Result<string>> CreateOrUpdateJournalAsync(string? journalId, JournalEntry journalEntry)
    {
        logger.LogInformation("Attempt to create Journal");
        //context.Database.BeginTransaction();
        try
        {
            var existingJournal = context.JournalEntries
                .Include(x => x.LoanTransaction)
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

            return Result.Success("New Journal Entry was Added.");
        }
        catch (Exception ex)
        {
            //context.Database.RollbackTransaction();
            logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<Result<LoanTransaction>> CreateJournalAsync(LoanTransaction model)
    {
        logger.LogInformation("Attempt to create Journal");
        try
        {
            await using var dataContext = await dbContextFactory.CreateDbContextAsync();
            var insertedEntity = dataContext.Add(model).Entity;
            await dataContext.SaveChangesAsync();
            return Result.Success(insertedEntity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<LoanTransaction>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<Result<JournalEntry>> CreateJournalAsync(JournalEntry model)
    {
        logger.LogInformation("Attempt to create Journal");

        await using var dataContext = await dbContextFactory.CreateDbContextAsync();

        try
        {
            dataContext.JournalEntries.Add(model);

            await dataContext.SaveChangesAsync();

            return Result.Success(model);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Result.Failure<JournalEntry>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }

    //
    public async Task<Result<string>> UpdateJournalAsync(string? id, LoanTransaction jTrx)
    {
        try
        {
            var trx = await context.LoanTransaction.Include(x => x.JournalEntry)
                .ThenInclude(x => x!.JournalEntryLines)
                .Where(x => x.TransactionsId == id)
                .FirstOrDefaultAsync();
            if (trx is null) return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));

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

            return Result.Success(Messages.DataCreated);
        }
        catch (Exception ex)
        {
            await context.Database.RollbackTransactionAsync();

            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }
    }

    public async Task<Result<string>> DeleteJournalAsync(string? id)
    {
        try
        {
            await context.Database.BeginTransactionAsync();

            var journalEntry = await context.JournalEntries.FirstOrDefaultAsync(x => x.JournalEntryId == id);

            if (journalEntry is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            context.JournalEntries.Remove(journalEntry);

            await context.SaveChangesAsync();

            await context.Database.CommitTransactionAsync();

            return Result.Success(Messages.DataDeleted);
        }
        catch (Exception ex)
        {
            await context.Database.RollbackTransactionAsync();

            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
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
            .OrderByDescendingIf(param.SortDescending, x => EF.Property<object>(x, param.SortColumn));
        var pageCount = await queries.CountAsync();
        var pageItems = await queries.Skip(param.Skip).Take(param.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return new Pager<JournalEntryVm>
        {
            Count = pageCount,
            Items = pageItems
        };
    }

    public async Task<IEnumerable<JournalEntry>> GetJournalsAsync(string? journalId, string? q = null)
    {
        IQueryable<JournalEntry> queries = context.JournalEntries
            .Include(x => x.LoanTransaction)
            .Include(x => x.JournalEntryLines);

        if (journalId.IsNotNullOrEmpty())
        {
            queries = queries.Where(x => x.JournalEntryId == journalId);
        }

        if (!string.IsNullOrEmpty(q))
        {
            var like = "%" + q + "%";

            queries = queries.Where(x => EF.Functions.Like(x.ReferrenceNum!, like) ||
                                         EF.Functions.Like(x.JournalType!, like) ||
                                         EF.Functions.Like(x.JournalEntryId, like) ||
                                         EF.Functions.Like(x.LoanTransaction!.TransactionDate!.ToString(), like));
        }

        return await queries.AsNoTracking().ToListAsync();
    }


    public async Task<Result<JournalEntryVm>> GetJournalByIdAsync(string? journalId, bool include)
    {
        //spGetJournalEntries
        if (journalId.IsNotNullOrEmpty())
        {
            return Result.Failure<JournalEntryVm>(
                ErrorResponse.NotFound($"There is no journal with this id:{journalId}."));
        }

        using var db = _dapperConnectionFactory.CreateConnection();

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
                ? Result.Failure<JournalEntryVm>(ErrorResponse.NotFound("Data Not Found."))
                : (Result<JournalEntryVm>)Result.Success(existingJournal);
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
            ? Result.Failure<JournalEntryVm>(ErrorResponse.NotFound("Data Not Found."))
            : Result.Success(item2);
    }

    public async Task<JournalEntry?> GetJournalByIdAsync(string? id)
    {
        var journal = context.JournalEntries
            .Where(x => x.JournalEntryId == id)
            .Include(x => x.LoanTransaction)
            .Include(a => a.JournalEntryLines);
        return await journal.AsNoTracking().FirstOrDefaultAsync();
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

    public async Task<IEnumerable<VNameList>> GetNameLists(long? id = 0, int takeRows = 0, string? q = null)
    {
        await using var context1 = await dbContextFactory.CreateDbContextAsync();

        IQueryable<VNameList> queries = context.NameLists;

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

        return await queries.AsNoTracking().ToListAsync();
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

        if (param.PageSize > 0)
        {
            queries = queries.Take(param.PageSize).OrderByDescending(x => x.FullName);
        }

        var items = await queries
            .ProjectToType<T>()
            .AsNoTracking()
            .ToListAsync();

        return items;
    }
}

//public async Task<JournalEntry?> FindJournal(Expression<Func<JournalEntry, bool>> expression, params string[] includes)
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
//    //return await PaginatedList<JournalEntryVM>.CreateAsync(journalEntries, pageNumber, pageSize);

//}