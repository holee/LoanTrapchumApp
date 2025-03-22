using Applications.Requests.Transactions.OpenBranches;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;

namespace Applications.Services.Transactions.Branches
{
    public interface IOpenBranchServiceImpl
    {
        Task<Result<string>> CloseBranchAsync(CloseBranchDtoUpdate transaction);
        Task<Result<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId);
        Task<Result<string>> OpenBranchAsync(OpenBranchDtoCreate transaction);
    }
}
