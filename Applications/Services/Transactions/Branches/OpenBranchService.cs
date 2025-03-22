using Applications.Interfaces.Transactions;
using Applications.Requests.Transactions.OpenBranches;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using Mapster;

namespace Applications.Services.Transactions.Branches
{

    public sealed class OpenBranchService(IOpenBranchService obService) : IOpenBranchServiceImpl
    {
        public async Task<Result<string>> CloseBranchAsync(CloseBranchDtoUpdate transaction)
        {
            var req = transaction.Adapt<LoanTransaction>();

            return await obService.CloseBranchAsync(req);
        }

        public async Task<Result<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId)
        {
            return await obService.GetCashVaultBalance(branchRefId);
        }

        public async Task<Result<string>> OpenBranchAsync(OpenBranchDtoCreate transaction)
        {
            var req = transaction.Adapt<LoanTransaction>();

            return await obService.OpenBranchAsync(req);
        }
    }
}
