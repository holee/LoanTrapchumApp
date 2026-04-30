using Applications.Commons;
using Applications.Features.Ledgers.Dto;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Ledgers;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Interfaces.Transactions;

public interface ILedgerService
{
    Task<ErrorOr<string>> CreateOrUpdateLedger(Ledger entity);
    Task<ErrorOr<string>> ChangeLedgerStatus(LedgerChangeStatus req);
    Task<ErrorOr<string>> DeleteLedger(int? id);
    Task<ErrorOr<Ledger>> GetLedgerByIdAsync(int? ledgerId);
    Task<ErrorOr<IEnumerable<LedgerType>>> FindLedgerTypes(int? ledgerTypeId = 0, string? q = null);
    Task<ErrorOr<LedgerType>> GetLedgerTypeById(int? ledgerId);
    Task<ErrorOr<IEnumerable<VLedger>>> GetLedgersAsync(int? ledgerId = 0, int? currencyId = 1, string? q = null);
    Task<Pager<VLedger>> GetLedgersAsync(int? pageIndex, int? pageSize, string sortColumn,
                                         bool sortDescending = false, int currencyId = 1,
                                         bool includeInactive = false, string? q = null);
    Task<Pager<VLedger>> GetLedgersAsync(int? currencyId, QueryParam q, CancellationToken cancellation = default);
}
