using Applications.Interfaces.Transactions;
using Applications.Requests.Journals;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Ledgers;
using Domains.Models.Journals;

namespace Applications.Services.Transactions.Journals;

public sealed class LedgerService(ILedgerService ledgService) : ILedgerService
{
    public async Task<Result<string>> ChangeLedgerStatus(LedgerChangeStatus req)
    {
        return await ledgService.ChangeLedgerStatus(req);
    }

    public async Task<Result<string>> CreateOrUpdateLedger(Ledger entity)
    {
        return await ledgService.CreateOrUpdateLedger(entity);
    }

    public async Task<Result<string>> DeleteLedger(int? id)
    {
        return await ledgService.DeleteLedger(id);
    }

    public async Task<Result<Ledger>> FindLedgerByIdAsync(int? ledgerId)
    {
        return await ledgService.FindLedgerByIdAsync(ledgerId);
    }

    public async Task<Result<LedgerType>> FindLedgerTypeById(int? ledgerId)
    {
        return await ledgService.FindLedgerTypeById(ledgerId);
    }

    public async Task<Result<IEnumerable<LedgerType>>> FindLedgerTypes(int? ledgerTypeId = 0, string? q = null)
    {
        return await ledgService.FindLedgerTypes(ledgerTypeId, q);
    }

    public async Task<Result<IEnumerable<VLedger>>> GetLedgersAsync(int? ledgerId = 0, int? currencyId = 1, string? q = null)
    {
        return await ledgService.GetLedgersAsync(ledgerId, currencyId, q);
    }

    public async Task<Pager<VLedger>> GetLedgersAsync(int? pageIndex, int? pageSize, string sortColumn, bool sortDescending = false, int currencyId = 1, bool includeInactive = false, string? q = null)
    {
        return await ledgService.GetLedgersAsync(pageIndex, pageSize, sortColumn, sortDescending, currencyId, includeInactive, q);
    }
}
