using Application.Features.Ledgers.Dto;
using Application.Interfaces.Transactions;
using ErrorOr;

namespace Application.Services.Transactions.Journals;


public interface ILedgerServiceImpl
{
    //Task<ErrorOr<string>> ChangeLedgerStatus(LedgerChangeStatus req);
    //Task<ErrorOr<string>> CreateOrUpdateLedger(Ledger entity);
    //Task<ErrorOr<string>> DeleteLedger(int? id);
    Task<ErrorOr<Ledger>> FindLedgerByIdAsync(int? ledgerId);
    //Task<ErrorOr<LedgerType>> FindLedgerTypeById(int? ledgerId);
    //Task<ErrorOr<IEnumerable<LedgerType>>> FindLedgerTypes(int? ledgerTypeId = 0, string? q = null);
   // Task<ErrorOr<IEnumerable<VLedger>>> GetLedgersAsync(int? ledgerId = 0, int? currencyId = 1, string? q = null);
    //Task<Pager<VLedger>> GetLedgersAsync(int? pageIndex, int? pageSize, string sortColumn, bool sortDescending = false, int currencyId = 1, bool includeInactive = false, string? q = null);
}

