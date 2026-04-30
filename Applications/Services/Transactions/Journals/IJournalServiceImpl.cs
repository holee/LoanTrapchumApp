using Applications.Features.Transactions.Journals;
using Applications.Responses.ViewModels.Journals;
using ErrorOr;

namespace Applications.Services.Transactions.Journals;

public interface IJournalServiceImpl
{
    Task<ErrorOr<string>> CreateOrUpdateJournalAsync(string? journalId, Domains.Models.Journals.JournalEntry journalEntry);
    Task<ErrorOr<string>> DeleteJournalAsync(string? id);
    Task<Domains.Models.Journals.JournalEntry?> GetJournalByIdAsync(string? id);
    Task<IEnumerable<JournalType>> GetJournalTypesAsync(string? expression = null);
    Task<Pager<JournalEntryVm>> GetJournalsAsync(int pageIndex, int pageSize, string sortColumn, bool sortDescending = false, string? q = null);
    Task<IEnumerable<Domains.Models.Journals.JournalEntry>> GetJournalsAsync(string? journalId, string? q = null);
    Task<IEnumerable<VNameList>> GetNamesList(long? id = 0, int takeRows = 0, string? q = null);
    Task<Pager<JournalEntryVm>> GetJournalsAsync(int? branchId, QueryParam param);
    Task<ErrorOr<JournalEntryVm>> GetJournalByIdAsync(string? id, bool include);
    Task<ErrorOr<string>> CreateJournalAsync(JournalTransactionDtoCreate req, CancellationToken cancellation = default);
}
