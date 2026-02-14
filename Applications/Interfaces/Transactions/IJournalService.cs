using Applications.Commons;
using Applications.Features.Journals.Dtos;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Domains.Models.Journals;
using Domains.Models.Transaction;
using ErrorOr;

namespace Applications.Interfaces.Transactions;

public interface IJournalService 
{
    Task<Result<string>> UpdateJournalAsync(string? journalId, JournalEntry journalEntry);
    Task<Result<LoanTransaction>> CreateJournalAsync(LoanTransaction model);
    Task<Result<JournalEntry>> CreateJournalAsync(JournalEntry model);
    Task<Result<string>> DeleteJournalAsync(string? id);
    Task<JournalEntry?> GetJournalByIdAsync(string? id);
    Task<Result<JournalEntryVm>> GetJournalByIdAsync(string? journalId, bool include);
    Task<IEnumerable<JournalType>> GetJournalTypesAsync(string? expression=null);
    Task<Pager<JournalEntryVm>> GetJournalsAsync(int pageIndex, int pageSize, string sortColumn, bool sortDescending = false, string? q = null);
    Task<IEnumerable<JournalEntry>> GetJournalsAsync(string? journalId, string? q = null);
    Task<IEnumerable<VNameList>> GetNameLists(long? id = 0, int takeRows = 0, string? q = null);
    Task<IEnumerable<T>> GetNameLists<T>(string? nameId, QueryParam param) where T : class, new();
    Task<Pager<JournalEntryVm>> GetJournalsAsync(int? branchId,QueryParam param);
    Task<Result<string>> CreateJournalAsync(LoanTransaction model, CancellationToken cancellation = default);
    Task<Result<string>> UpdateJournalAsync(LoanTransaction tranx, CancellationToken cancellation = default);
    Task<Pager<JournalTypeDto>> GetJournalTypesAsync(int? journalTypeId, QueryParam param, CancellationToken cancellation = default);
    Task<PaginationResult<JournalEntryDto>> GetJournalsAsync(string? journalId, QueryParam param, CancellationToken cancellation = default);
    Task<JournalEntryDto?> GetJournalDetailByIdAsync(string? journalEntryId, CancellationToken cancellation = default);
    Task<JournalEntry?> GetJournalByIdAsync(string? journalEntryId, CancellationToken cancellation = default);
    Task<ErrorOr<JournalEntry?>> CreateAsync(LoanTransaction model, CancellationToken cancellation = default);
}
