using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies.Responses;
using Applications.Responses;
using Domains.Models.Transaction;

namespace Applications.Interfaces.Transactions
{
    public interface ICashVaultService
    {
        Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId, CancellationToken cancellation = default);
        Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync(CancellationToken cancellation = default);
        Task<IEnumerable<VCurrencyNotes>> GetCurrencyNotesAsync(int? branchId,CancellationToken cancellation = default);
        Task<Result> CreateCashManagementAsync(LoanTransaction cashTrsansaction, CancellationToken cancellation = default); 
        Task<Result> CreateCashVaultAsync(LoanTransaction transaction, CancellationToken cancellation = default);
    }
}
