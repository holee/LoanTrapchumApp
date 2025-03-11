using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Domains.Models.Lists;

namespace Applications.Interfaces.Branches
{
    public interface IBranchService
    {
        Task<Result> Create(Branch branch);
        Task<Result> Update(Branch branch);
        Task<Result> UpdateStatusAsync(int branchId);
        Task<Result> Delete(int id);
        Task<Result<Branch>> GetBranchById(int id);
        Task<Result<Branch>> GetBranchForVault(int id);
        Task<Result<IEnumerable<VBranch>>> GetBranchesByUser(long userId,int branchId=0); 
        Task<Pager<VBranch>> GetBranchesAsync(QueryParam param);
        Task<Result<ResultStatus>> GetBrachStatus(int branchRefId);
        Task<Result<IEnumerable<VBranch>>> GetBranchesAsync(int branchId = 0, int pageSize = 0, bool includeInactive = false, string? filter = null);
        Task<Result<IEnumerable<VBranch>>> GetBranchesAsync(int branchId = 0, int take = 0, string? filter = null);
    }
}
