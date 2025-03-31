using Applications.Interfaces.Branches;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Domains.Models.Lists;
using Mapster;

namespace Applications.Services.Branches
{
    public interface IBranchServiceImpl
    {
        Task<Result> Create(Branch branch);
        Task<Result> DeleteAsync(int? id);
        Task<Result<ResultStatus>> GetBrachStatus(int? branchRefId);
        Task<Result<Branch>> GetBranchById(int? id);
        Task<Pager<VBranchResponse>> GetBranchesAsync(QueryParam param);
        Task<Result<IEnumerable<VBranchResponse>>> GetBranchesAsync(int branchId = 0, int pageSize = 0, bool includeInactive = false, string? filter = null);
        Task<Result<IEnumerable<VBranchResponse>>> GetBranchesAsync(int? branchId = 0, int? pageSize = 0, bool includeInactive = false, string? filter = null);
        Task<Result<IEnumerable<VBranchResponse>>> GetBranchesByUser(string userId, int? branchId = 0);
        Task<Result<Branch>> GetBranchForVault(int? id);
        Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId);
        Task<Result> Update(Branch branch);
        Task<Result> UpdateStatusAsync(int? branchId);
    }
    public sealed class BranchService(IBranchService branchService) : IBranchServiceImpl
    {
        public async Task<Result> Create(Branch branch)
        {
            return await branchService.Create(branch);
        }

        public async Task<Result> DeleteAsync(int? id)
        {
            return await branchService.DeleteAsync(id);
        }

        public async Task<Result<ResultStatus>> GetBrachStatus(int? branchRefId)
        {
            return await branchService.GetBrachStatus(branchRefId);
        }

        public async Task<Result<Branch>> GetBranchById(int? id)
        {
            return await branchService.GetBranchById(id);
        }

        public async Task<Pager<VBranchResponse>> GetBranchesAsync(QueryParam param)
        {
            return await branchService.GetBranchesAsync(param);
        }

        public async Task<Result<IEnumerable<VBranchResponse>>> GetBranchesAsync(int branchId = 0, int pageSize = 0, bool includeInactive = false, string? filter = null)
        {
            return await branchService.GetBranchesAsync(branchId, pageSize, includeInactive, filter);
        }


        public Task<Result<IEnumerable<VBranchResponse>>> GetBranchesAsync(int? branchId = 0, int? pageSize = 0, bool includeInactive = false, string? filter = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<VBranchResponse>>> GetBranchesByUser(string userId, int? branchId = 0)
        {
            return await branchService.GetBranchesByUser(userId, branchId);
        }

        public async Task<Result<Branch>> GetBranchForVault(int? id)
        {
            return await branchService.GetBranchForVault(id);
        }

        public async Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId)
        {
            return await branchService.GetUpdateBranchById(branchId);
        }

        public async Task<Result> Update(Branch branch)
        {
            return await branchService.Update(branch);
        }

        public async Task<Result> UpdateStatusAsync(int? branchId)
        {
            return await branchService.UpdateStatusAsync(branchId);
        }


    }
}
