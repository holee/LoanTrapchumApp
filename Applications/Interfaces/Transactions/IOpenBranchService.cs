using Applications.Requests.Transactions.LoanTransactions;
using Applications.Requests.Transactions.OpenBranches;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
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
