using Applications.Requests.Journals;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Ledgers;
using Domains.Models.Journals;

namespace Applications.Services.Transactions;

public interface ILedgerService
{
    Task<Result<string>> CreateOrUpdateLedger(Ledger entity);
    Task<Result<string>> ChangeLedgerStatus(LedgerChangeStatus req);
    Task<Result<string>> DeleteLedger(int id);
    Task<Result<Ledger>> FindLedgerByIdAsync(int ledgerId);
    Task<Result<IEnumerable<LedgerType>>> FindLedgerTypes(int? ledgerTypeId = 0, string? q = null);
    Task<Result<LedgerType>> FindLedgerTypeById(int ledgerId);
    Task<Result<IEnumerable<VLedger>>> GetLedgersAsync(int ledgerId = 0, int currencyId = 1, string? q = null);
    Task<Pager<VLedger>> GetLedgersAsync(int pageIndex, int pageSize, string sortColumn
                                         , bool sortDescending = false, int currencyId = 1,
                                          bool includeInactive = false, string? q = null);
}
