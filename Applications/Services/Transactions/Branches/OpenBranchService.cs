using Applications.Features.CashManagements.Responses;
using Applications.Features.Transactions.OpenBranches;
using Applications.Features.Transactions.OpenBranches;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Domains.Models.Transaction;
using Mapster;

namespace Applications.Services.Transactions.Branches
{

    public sealed class OpenBranchService(IOpenBranchService obService) : IOpenBranchServiceImpl
    {
        //public async Task<Result<string>> CloseBranchAsync(TransactionCloseBranchDto transaction)
        //{
        //    var req = transaction.Adapt<LoanTransaction>();

        //    return await obService.CloseBranchAsync(req);
        //}

        //public async Task<Result<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId)
        //{
        //    return await obService.GetCashVaultBalance(branchRefId);
        //}

        //public async Task<Result<string>> OpenBranchAsync(OpenBranchTransactionDto transaction) 
        //{
        //    var req = transaction.Adapt<LoanTransaction>();

        //    return await obService.OpenBranchAsync(req);
        //}
    }
}
