using Application.Features.CashManagements.Responses;
using Application.Features.Currencies.Responses;
using Application.Responses;
using Domains.Models.Transaction;

namespace Application.Interfaces.Transactions
{
    public interface ICashVaultService
    {
        Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId, CancellationToken cancellation = default);
        Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync(CancellationToken cancellation = default);
        Task<IEnumerable<VCurrencyNotes>> GetCurrencyNotesAsync(int? branchId,CancellationToken cancellation = default);
        Task<Result> CreateCashManagementAsync(LoanTransaction cashTrsansaction, CancellationToken cancellation = default); 
        Task<Result> CreateCashVaultAsync(LoanTransaction transaction, CancellationToken cancellation = default);
        Task<Pager<VCashVaults>> GetViewCashVaultAsync(int? branchId, CancellationToken cancellation = default);
        Task<ErrorOr.ErrorOr<string>> CashVaultAsync(LoanTransaction trnx, CancellationToken cancellation = default);
    }
}
