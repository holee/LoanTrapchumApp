using Applications.Features.Ledgers.Dto;
using Applications.Interfaces.Transactions;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Ledgers;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Services.Transactions.Journals;


public interface ILedgerServiceImpl
{
    Task<ErrorOr<string>> ChangeLedgerStatus(LedgerChangeStatus req);
    Task<ErrorOr<string>> CreateOrUpdateLedger(Ledger entity);
    Task<ErrorOr<string>> DeleteLedger(int? id);
    Task<ErrorOr<Ledger>> FindLedgerByIdAsync(int? ledgerId);
    Task<ErrorOr<LedgerType>> FindLedgerTypeById(int? ledgerId);
    Task<ErrorOr<IEnumerable<LedgerType>>> FindLedgerTypes(int? ledgerTypeId = 0, string? q = null);
    Task<ErrorOr<IEnumerable<VLedger>>> GetLedgersAsync(int? ledgerId = 0, int? currencyId = 1, string? q = null);
    Task<Pager<VLedger>> GetLedgersAsync(int? pageIndex, int? pageSize, string sortColumn, bool sortDescending = false, int currencyId = 1, bool includeInactive = false, string? q = null);
}

public sealed class LedgerService(ILedgerService ledgService) : ILedgerServiceImpl
{
    public async Task<ErrorOr<string>> ChangeLedgerStatus(LedgerChangeStatus req)
    {
        return await ledgService.ChangeLedgerStatus(req);
    }

    public async Task<ErrorOr<string>> CreateOrUpdateLedger(Ledger entity)
    {
        return await ledgService.CreateOrUpdateLedger(entity);
    }

    public async Task<ErrorOr<string>> DeleteLedger(int? id)
    {
        return await ledgService.DeleteLedger(id);
    }

    public async Task<ErrorOr<Ledger>> FindLedgerByIdAsync(int? ledgerId)
    {
        return await ledgService.GetLedgerByIdAsync(ledgerId);
    }

    public async Task<ErrorOr<LedgerType>> FindLedgerTypeById(int? ledgerId)
    {
        return await ledgService.GetLedgerTypeById(ledgerId);
    }

    public async Task<ErrorOr<IEnumerable<LedgerType>>> FindLedgerTypes(int? ledgerTypeId = 0, string? q = null)
    {
        return await ledgService.FindLedgerTypes(ledgerTypeId, q);
    }

    public async Task<ErrorOr<IEnumerable<VLedger>>> GetLedgersAsync(int? ledgerId = 0, int? currencyId = 1, string? q = null)
    {
        return await ledgService.GetLedgersAsync(ledgerId, currencyId, q);
    }

    public async Task<Pager<VLedger>> GetLedgersAsync(int? pageIndex, int? pageSize, string sortColumn, bool sortDescending = false, int currencyId = 1, bool includeInactive = false, string? q = null)
    {
        return await ledgService.GetLedgersAsync(pageIndex, pageSize, sortColumn, sortDescending, currencyId, includeInactive, q);
    }
}
