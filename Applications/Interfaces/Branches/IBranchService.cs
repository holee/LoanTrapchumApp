using Applications.Requests.Lists.Entities;
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
        Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId);
        Task<Result> UpdateStatusAsync(int? branchId);
        Task<Result> DeleteAsync(int? id);
        Task<Result<Branch>> GetBranchById(int? id);
        Task<Result<T>> GetBranchById<T>(int? id);
        Task<Result<Branch>> GetBranchForVault(int? id);
        Task<Result<IEnumerable<VBranchResponse>>> GetBranchesByUser(string userId, int? branchId);
        Task<Pager<VBranchResponse>> GetBranchesAsync(QueryParam param);
        Task<Result<ResultStatus>> GetBrachStatus(int? branchRefId);
        Task<Result<IEnumerable<VBranchResponse>>> GetBranchesAsync(int? branchId = 0, int? pageSize = 0, bool includeInactive = false, string? filter = null);
        Task<Result<IEnumerable<T>>> GetBranchesAsync<T>(int? branchId, int? pageSize,
            bool includeInactive = false, string? filter = null)
            where T : class;
    }
}
