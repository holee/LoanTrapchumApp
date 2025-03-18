using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Journals;
using Domains.Models.Journals;
using Domains.Models.Transaction;

namespace Applications.Services.Transactions.Journals; 

public sealed class JournalService(IJournalService jService) : IJournalService
{
    public async Task<Result<LoanTransaction>> CreateJournalAsync(LoanTransaction model)
    {
        return await jService.CreateJournalAsync(model);
    }

    public async Task<Result<string>> CreateOrUpdateJournalAsync(long? journalId, JournalEntry journalEntry)
    {
        return await jService.CreateOrUpdateJournalAsync(journalId, journalEntry);
    }

    public async Task<Result<string>> DeleteJournalAsync(long? id)
    {
        return await jService.DeleteJournalAsync(id);
    }

    public async Task<JournalEntry?> FindJournalByIdAsync(long? id)
    {
        return await jService.FindJournalByIdAsync(id);
    }

    public async Task<Pager<VJournal>> FindJournalsAsync(int pageIndex, int pageSize, string sortColumn, bool sortDescending = false, string? q = null)
    {
        return await jService.FindJournalsAsync(pageIndex, pageSize, sortColumn, sortDescending, q);
    }

    public async Task<IEnumerable<JournalEntry>> FindJournalsAsync(long? journalId = 0, string? q = null)
    {
        return await jService.FindJournalsAsync(journalId, q);
    }

    public async Task<IEnumerable<JournalType>> FindJournalTypesAsync(string? expression = null)
    {
        return await jService.FindJournalTypesAsync(expression);
    }

    public async Task<IEnumerable<VNameList>> FindNameLists(long? id = 0, int takeRows = 0, string? q = null)
    {
        return await jService.FindNameLists(id, takeRows, q);
    }
}
