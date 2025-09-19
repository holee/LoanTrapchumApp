using Applications.Features.Transactions.Journals;
using Applications.Features.Transactions.Journals;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Domains.Models.Journals;
using Domains.Models.Transaction;

namespace Applications.Services.Transactions.Journals;

public interface IJournalServiceImpl 
{
    Task<Result<string>> CreateOrUpdateJournalAsync(string? journalId, JournalEntry journalEntry);
    Task<Result<string>> DeleteJournalAsync(string? id);
    Task<JournalEntry?> GetJournalByIdAsync(string? id);
    Task<IEnumerable<JournalType>> GetJournalTypesAsync(string? expression = null);
    Task<Pager<JournalEntryVm>> GetJournalsAsync(int pageIndex, int pageSize, string sortColumn, bool sortDescending = false, string? q = null);
    Task<IEnumerable<JournalEntry>> GetJournalsAsync(string? journalId, string? q = null);
    Task<IEnumerable<VNameList>> GetNamesList(long? id = 0, int takeRows = 0, string? q = null); 
    
    Task<Pager<JournalEntryVm>> GetJournalsAsync(int? branchId,QueryParam param);
    Task<Result<JournalEntryVm>> GetJournalByIdAsync(string? id, bool include);
    Task<Result<string>> CreateJournalAsync(JournalTransactionDtoCreate req, CancellationToken cancellation = default);
}
