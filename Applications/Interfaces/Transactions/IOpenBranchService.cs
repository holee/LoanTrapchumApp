using Applications.Features.CashManagements.Responses;
using Applications.Responses;
using Domains.Models.Transaction;

namespace Applications.Interfaces.Transactions
{
    public interface IOpenBranchService
    {
        Task<Result<string>> CloseBranchAsync(LoanTransaction transaction);
        Task<Result<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId);
        Task<Result<string>> OpenBranchAsync(LoanTransaction transaction);
    }
}
