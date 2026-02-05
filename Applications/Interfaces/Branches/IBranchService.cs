using Applications.Features.Branches.Dtos;
using Applications.Responses;
using Applications.Responses.Auths;
using ErrorOr;
using System.Linq.Expressions;
namespace Applications.Interfaces.Branches
{
    public interface IBranchService
    {
        Task<ErrorOr<VBranch>> CreateAsync(Branch branch, CancellationToken cancellation = default);
        Task<ErrorOr<VBranch>> UpdateAsync(Branch branch, CancellationToken cancellation = default);
        Task<ErrorOr<BranchDtoUpdate>> GetUpdateBranchById(int? branchId, CancellationToken cancellation = default);
        Task<ErrorOr<VBranch>> UpdateStatusAsync(int? branchId, CancellationToken cancellation = default);
        Task<ErrorOr<VBranch>> UpdateStatusAsync(int? branchId, bool state, CancellationToken cancellation = default);
        Task<ErrorOr<DeleteResult>> DeleteAsync(int? id, CancellationToken cancellation = default);
        Task<ErrorOr<Branch>> GetBranchById(int? id, CancellationToken cancellation = default);
        Task<ErrorOr<VBranch>> GetBranchForVault(int? id, CancellationToken cancellation = default);
        Task<ErrorOr<UserBranchesResponse?>> GetBranchesByUser(string? userId, CancellationToken cancellation = default);     
        Task<ErrorOr<IEnumerable<VBranch>>> GetBranchesByUser(string? userId, int? branchId, CancellationToken cancellation = default);
        Task<ErrorOr<BranchStatus>> GetBranchStatus(int? branchId, CancellationToken cancellation = default);
       
        Task<Pager<VBranch>> GetBranchesAsync(int? branchId, QueryParam param, CancellationToken cancellation = default);
        Task<ErrorOr<IEnumerable<ApprovedUser>>> GetApprovedUser(int? branchId, string? filter, CancellationToken cancellation = default);
        Task<VBranch?> GetBranchDetailsById(int? branchId, Expression<Func<VBranch, object>> selector, CancellationToken cancellation = default);
        Task<VBranch?> GetBranchDetailsById(int? branchId, CancellationToken cancellation = default, params string[] columns);
        Task<Pager<VBranch>> GetBranchesAsync(int? branchId, QueryParam param, CancellationToken cancellation = default, params string[] columns);
        Task<ErrorOr<BranchStatus>> GetBranchStatus(string? userId, CancellationToken cancellation = default);
    }
}
