using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Domains.Models.Lists;

namespace Applications.Interfaces.Branches
{
    public interface IBranchService
    {
        Task<Result> Create(Branch branch);
        Task<Result> UpdateAsync(Branch branch);
        Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId);
        Task<Result> UpdateStatusAsync(int? branchId);
        Task<Result> DeleteAsync(int? id);
        Task<Result<Branch>> GetBranchById(int? id);
        Task<Result<T>> GetBranchById<T>(int? id) where T:class;
        Task<Result<Branch>> GetBranchForVault(int? id);
        Task<Result<IEnumerable<VBranchResponse>>> GetBranchesByUser(string? userId, int? branchId);
      
        Task<Result<ResultStatus>> GetBrachStatus(int? branchId);
        Task<Result<UserBranchesResponse>> GetBranchesByUser(string userId);
        Task<Pager<VBranchResponse>> GetBranchesAsync(QueryParam param);
        Task<Result<Pager<T>>> GetBranchesAsync<T>(int? branchId, QueryParam param) where T : class;
        Task<Result<IEnumerable<T>>> GetBranchesByUser<T>(string? userId, int? branchId = 0) where T : class;
        Task<Result<IEnumerable<ApprovedUser>>> GetApprovedUser(int? branchId, string? filter);
        Task<Result<string>> UpdateStatusAsync(int? branchId, bool state);
    }
}
