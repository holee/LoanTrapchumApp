using Applications.Features.CashManagements.Responses;
using Applications.Responses;
using Domains.Models.Transaction;
using ErrorOr;

namespace Applications.Interfaces.Transactions
{
    public interface IOpenBranchService
    {
        Task<ErrorOr<BranchStatus>> CloseBranchAsync(LoanTransaction transaction, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId, CancellationToken cancellation = default);
        Task<ErrorOr<BranchStatus>> OpenBranchAsync(LoanTransaction transaction, CancellationToken cancellation = default);
    }
}
