using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;

namespace Applications.Interfaces.Transactions
{
    public interface IOpenBranchServiceImpl
    {
        Task<Result<string>> CloseBranchAsync(LoanTransaction transaction);
        Task<Result<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int branchRefId);
        Task<Result<string>> OpenBranchAsync(LoanTransaction transaction);
    }
}
