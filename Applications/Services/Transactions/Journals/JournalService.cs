using Applications.Commons.Mappers;
using Applications.Features.Transactions.Journals;
using Applications.Interfaces.Transactions;
using Applications.Responses.ViewModels.Journals;

namespace Applications.Services.Transactions.Journals;

public sealed class JournalService(IJournalService jService) : IJournalServiceImpl
{

    public async Task<Result<string>> CreateJournalAsync(JournalTransactionDtoCreate req,CancellationToken cancellation=default)
    {
        var model = req.MapTransactionCreate();

        return await jService.CreateJournalAsync(model,cancellation);
    }

    //
    public async Task<Result<string>> CreateOrUpdateJournalAsync(string? journalId, Domains.Models.Journals.JournalEntry journalEntry)
    {
        return await jService.UpdateJournalAsync(journalId, journalEntry);
    }

    public async Task<Result<string>> DeleteJournalAsync(string? id)
    {
        return await jService.DeleteJournalAsync(id);
    }

    public async Task<Domains.Models.Journals.JournalEntry?> GetJournalByIdAsync(string? id)
    {
        return await jService.GetJournalByIdAsync(id);
    }

    public async Task<Result<JournalEntryVm>> GetJournalByIdAsync(string? id, bool include)
    {
        return await jService.GetJournalByIdAsync(id, include);;
    }

    public async Task<Pager<JournalEntryVm>> GetJournalsAsync(int pageIndex, int pageSize, string sortColumn,
        bool sortDescending = false, string? q = null)
    {
        return await jService.GetJournalsAsync(pageIndex, pageSize, sortColumn, sortDescending, q);
    }

    public async Task<IEnumerable<Domains.Models.Journals.JournalEntry>> GetJournalsAsync(string? journalId, string? q = null)
    {
        return await jService.GetJournalsAsync(journalId, q);
    }

    public async Task<IEnumerable<JournalType>> GetJournalTypesAsync(string? expression = null)
    {
        return await jService.GetJournalTypesAsync(expression);
    }

    public async Task<Pager<JournalEntryVm>> GetJournalsAsync(int? branchId,QueryParam param)
    {
        return await jService.GetJournalsAsync(branchId,param);
    }

    public async Task<IEnumerable<VNameList>> GetNamesList(long? id = 0, int takeRows = 0, string? q = null)
    {
        return await jService.GetNameLists(id, takeRows, q);
    }
    
}