using Applications.Interfaces.Transactions;

namespace Applications.Services.Transactions.Branches
{

    public sealed class OpenBranchService() : IOpenBranchServiceImpl
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
