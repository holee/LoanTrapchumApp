using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Domains.Models.Journals;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public class JournalService : IJournalService
{
    private readonly ApplicationDbContext _context;
    private ILogger<JournalService> _logger;
    public JournalService(ApplicationDbContext context, ILogger<JournalService> logger)
    {
        this._context = context;
        this._logger = logger;
    }
    public async Task<Result<string>> CreateOrUpdateJournalAsync(long? journalId, JournalEntry journalEntry)
    {
        _logger.LogInformation("Attempt to create Journal");
        //context.Database.BeginTransaction();
        try
        {

            var existingJournal = _context.JournalEntries
                                        .Include(x => x.LoanTransaction)
                                        .Include(j => j.JournalEntryLines)
                                        .FirstOrDefault(x => x.JournalId == journalEntry.JournalId);

            JournalEntry? insertedEntity = null;

            if (existingJournal is null)
            {
                insertedEntity = _context.Add(journalEntry).Entity;
            }
            else
            {

                _context.Entry(existingJournal)
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
                        _context.Entry(existingJounralLine).CurrentValues.SetValues(line);
                    }
                }

                foreach (var item in existingJournal.JournalEntryLines!)
                {
                    if (!journalEntry.JournalEntryLines.Any(x => x.JournalLineId == item.JournalLineId))
                    {
                        _context.Remove(item);
                    }

                }
            }
            await _context.SaveChangesAsync();
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
            _logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }


    public async Task<Result<LoanTransaction>> CreateJournalAsync(LoanTransaction model)
    {
        _logger.LogInformation("Attempt to create Journal");
        try
        {
            var insertedEntity = _context.Add(model).Entity;
            await _context.SaveChangesAsync();
            return Result.Success(insertedEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Result.Failure<LoanTransaction>(ErrorResponse.ServerError($"{ex.Message}"));
        }
    }



    public async Task<Result<string>> UpdateJournalAsync(long? id, LoanTransaction jTrx)
    {
        try
        {
            var trx = await _context.LoanTransaction.Include(x => x.JournalEntry)
                                                    .ThenInclude(x => x!.JournalEntryLines)
                                                        .Where(x => x.TranId == id)
                                                        .FirstOrDefaultAsync();
            if (trx is null) return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));

            _context.Database.BeginTransaction();
            trx.TransactionTypeName = jTrx.TransactionTypeName;
            var lines = trx.JournalEntry?.JournalEntryLines;
            _context.JournalEntryLines.RemoveRange(lines!);
            await _context.SaveChangesAsync();
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

            _context.Update(trx);

            await _context.SaveChangesAsync();

            _context.Database.CommitTransaction();

            return Result.Success(Messages.DataCreated);

        }
        catch (Exception ex)
        {
            _context.Database.RollbackTransaction();

            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }
    }

    public async Task<Result<string>> DeleteJournalAsync(long? id)
    {
        try
        {
            _context.Database.BeginTransaction();

            var journalEntry = await _context.JournalEntries.FirstOrDefaultAsync(x => x.JournalId == id);

            if (journalEntry is null)
            {
                return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));

            }

            _context.JournalEntries.Remove(journalEntry);

            await _context.SaveChangesAsync();

            _context.Database.CommitTransaction();

            return Result.Success(Messages.DataDeleted);

        }
        catch (Exception ex)
        {
            _context.Database.RollbackTransaction();

            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }
    }

    public async Task<Pager<VJournal>> FindJournalsAsync(int pageIndex, int pageSize,
        string sortColumn, bool sortDescending = false, string? q = null)
    {

        IQueryable<VJournal> queries = _context.VJournals;

        if (!string.IsNullOrEmpty(q))
        {

            var like = "%" + q + "%";

            queries = queries.Where(x => EF.Functions.Like(x.ReferrenceNum!, like) ||
                                         EF.Functions.Like(x.JournalType!, like) ||
                                         EF.Functions.Like(x.TransactionTypeName!, like) ||
                                         EF.Functions.Like(x.TransactionDate!.ToString(), like));
        }

        if (sortDescending)
        {
            queries = queries.OrderByDescending(x => EF.Property<object>(x, sortColumn));
        }
        else
        {
            queries = queries.OrderBy(x => EF.Property<object>(x, sortColumn));
        }

        var pageCount = await queries.CountAsync();
        var pageItems = queries.Skip(pageIndex).Take(pageSize).ToList();

        return new Pager<VJournal>
        {
            Count = pageCount,
            Items = pageItems
        };


    }

    public async Task<IEnumerable<JournalEntry>> FindJournalsAsync(long? journalId = 0, string? q = null)
    {

        IQueryable<JournalEntry> queries = _context.JournalEntries
                                                   .Include(x => x.LoanTransaction)
                                                   .Include(x => x.JournalEntryLines);

        if (journalId > 0)
        {
            queries = queries.Where(x => x.JournalId == journalId);
        }

        if (!string.IsNullOrEmpty(q))
        {

            var like = "%" + q + "%";

            queries = queries.Where(x => EF.Functions.Like(x.ReferrenceNum!, like) ||
                                         EF.Functions.Like(x.JournalType!, like) ||
                                         EF.Functions.Like(x.JournalId!.ToString(), like) ||
                                         EF.Functions.Like(x.LoanTransaction!.TransactionDate!.ToString(), like));
        }

        return await queries.AsNoTracking().ToListAsync();


    }

    public async Task<JournalEntry?> FindJournalByIdAsync(long? id)
    {
        var journal = _context.JournalEntries
                                .Where(x => x.JournalId == id)
                                .Include(x => x.LoanTransaction)
                                .Include(a => a.JournalEntryLines);
        return await journal.AsNoTracking().FirstOrDefaultAsync();
    }


    public async Task<IEnumerable<JournalType>> FindJournalTypesAsync(string? expression = null)
    {
        IQueryable<JournalType> journalTypes = _context.JournalTypes;
        if (!string.IsNullOrEmpty(expression))
        {
            journalTypes = journalTypes.Where(x =>
               x.Code != null && x.Code.ToLower().StartsWith(expression)
               || x.Description != null && x.Description.ToLower().StartsWith(expression));
        }
        return await journalTypes.AsNoTracking().ToListAsync() ?? Enumerable.Empty<JournalType>();
    }

    public async Task<IEnumerable<VNameList>> FindNameLists(long? id = 0, int takeRows = 0, string? q = null)
    {
        IQueryable<VNameList> queries = _context.NameLists;

        if (id > 0)
        {
            queries = queries.Where(x => x.Id == id);
        }

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