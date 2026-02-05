using Applications.Features.Branches.Dtos;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;

namespace Applications.Features.Branches.Interfaces
{
    public interface IBranchServiceImpl
    {
        Task<Result> CreateAsync(Branch branch);
        Task<Result> DeleteAsync(int? id);
        Task<Result> UpdateAsync(BranchDtoUpdate branch);
        Task<Result<BranchStatus>> GetBrachStatus(int? branchRefId);
        Task<Result<Branch>> GetBranchById(int? id);
        Task<Pager<VBranch>> GetBranchesAsync(QueryParam param);
        Task<Result<Pager<T>>> GetBranchesAsync<T>(int? branchId, QueryParam param) where T : class;
        Task<Result<IEnumerable<VBranch>>> GetBranchesByUser(string? userId, int? branchId);
        Task<Result<UserBranchesResponse>> GetBranchesByUser(string? userId);
        Task<Result<Branch>> GetBranchForVault(int? id);
        Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId);
        Task<Result> UpdateStatusAsync(int? branchId);
        Task<Result<T>> GetBranchById<T>(int? id) where T : class;
        Task<Result<string>> UpdateStatusAsync(int? branchId, bool state);
    }
}