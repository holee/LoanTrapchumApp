using Application.Features.CashManagements.Responses;
using Application.Responses;
using Domains.Models.Transaction;
using ErrorOr;

namespace Application.Interfaces.Transactions
{
    public interface IOpenBranchService
    {
        Task<ErrorOr<BranchStatus>> CloseBranchAsync(LoanTransaction transaction, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId, CancellationToken cancellation = default);
        Task<ErrorOr<BranchStatus>> OpenBranchAsync(LoanTransaction transaction, CancellationToken cancellation = default);
    }
}
