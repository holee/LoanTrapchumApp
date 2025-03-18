using Applications.Interfaces.Transactions;
using Applications.Requests.Transactions.OpenBranches;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;

namespace Applications.Services.Transactions.Branches
{
    public sealed class OpenBranchService(IOpenBranchService obService) : IOpenBranchService
    {
        public async Task<Result<string>> CloseBranchAsync(LoanTransaction transaction)
        {
            return await obService.CloseBranchAsync(transaction);
        }

        public async Task<Result<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId)
        {
            return await obService.GetCashVaultBalance(branchRefId);
        }

        public async Task<Result<string>> OpenBranchAsync(LoanTransaction transaction)
        {
            return await obService.OpenBranchAsync(transaction);
        }
    }
}
