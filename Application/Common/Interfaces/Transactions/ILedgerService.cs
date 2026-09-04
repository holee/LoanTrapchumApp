using Application.Commons;
using Application.Features.Ledgers.Dto;
using Application.Responses.Paginations;
using Domains.Models.Journals;
using ErrorOr;
using System.Linq.Expressions;

namespace Application.Interfaces.Transactions;

public interface ILedgerService
{
    Task<VLedger> CreateOrUpdateLedger(Ledger entity, CancellationToken cancellation = default);
    Task<ErrorOr<string>> ChangeLedgerStatus(LedgerChangeStatus req, CancellationToken cancellation = default);
    Task<ErrorOr<string>> DeleteLedger(int? id, CancellationToken cancellation = default);
    Task<Ledger?> GetLedgerByIdAsync(int? ledgerId, CancellationToken cancellation = default);
    Task<PageMetaData<LedgerType>> GetLedgerTypes(int? ledgerTypeId, QueryParam param, CancellationToken cancellation = default);
    Task<LedgerType?> GetLedgerTypeById(int? ledgerId, CancellationToken cancellation = default);
    //Task<Pager<VLedger>> GetLedgersAsync(int? currencyId, QueryParam q, CancellationToken cancellation = default);
    Task<PageMetaData<VLedger>> GetLedgersAsync(int? ledgerId, int? currencyId, QueryParam? param, CancellationToken cancellation = default, params string[] columns);
    Task<PageMetaData<VLedger>> GetLedgersAsync(int? currencyId, QueryParam query, CancellationToken cancellation = default, params string[] columns);
    Task<VLedger?> GetLedgerByIdAsync(int? ledgerId, CancellationToken cancellation = default, params string[] columns);
    Task<DeleteResult> DeleteAsync(int? id, CancellationToken cancellation = default);
    Task<Ledger?> GetLedgerByIdAsync(int? ledgerId, Expression<Func<Ledger, object>>? include, CancellationToken cancellation = default);
}
