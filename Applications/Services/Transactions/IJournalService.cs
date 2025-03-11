using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Domains.Models.Journals;
using Domains.Models.Transaction;

namespace Applications.Services.Transactions;

public interface IJournalService
{
    Task<Result<string>> CreateOrUpdateJournalAsync(long journalId, JournalEntry journalEntry);
    Task<Result<LoanTransaction>> CreateJournalAsync(LoanTransaction model);
    Task<bool> DeleteJournalAsync(long id);
    Task<JournalEntry?> FindJournalByIdAsync(long id);
    Task<IEnumerable<JournalType>> FindJournalTypesAsync(string? expression=null);
    Task<Pager<VJournal>> FindJournalsAsync(int pageIndex, int pageSize, string sortColumn, bool sortDescending = false, string? q = null);
    Task<IEnumerable<JournalEntry>> FindJournalsAsync(long journalId = 0, string? q = null);
    Task<IEnumerable<VNameList>> FindNameLists(long id = 0, int takeRows = 0, string? q = null);
    
}
