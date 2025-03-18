using Applications.Interfaces.Branches;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Branches;
using Domains.Models.Lists;
using Mapster;

namespace Applications.Services.Branches
{
    public sealed class BranchService : IBranchService
    {
        private readonly IBranchService _branchService;

        public BranchService(IBranchService branchService)
        {
            _branchService = branchService;
        }

        public async Task<Result> Create(Branch branch)
        {
            return await _branchService.Create(branch);
        }

        public async Task<Result> DeleteAsync(int? id)
        {
            return await _branchService.DeleteAsync(id);
        }

        public async Task<Result<ResultStatus>> GetBrachStatus(int? branchRefId)
        {
            return await _branchService.GetBrachStatus(branchRefId);
        }

        public async Task<Result<Branch>> GetBranchById(int? id)
        {
            return await _branchService.GetBranchById(id);
        }

        public async Task<Pager<VBranch>> GetBranchesAsync(QueryParam param)
        {
            return await _branchService.GetBranchesAsync(param);
        }

        public async Task<Result<IEnumerable<VBranch>>> GetBranchesAsync(int branchId = 0, int pageSize = 0, bool includeInactive = false, string? filter = null)
        {
            return await _branchService.GetBranchesAsync(branchId, pageSize, includeInactive, filter);
        }

        public async Task<Result<IEnumerable<VBranch>>> GetBranchesAsync(int branchId = 0, int take = 0, string? filter = null)
        {
            return await _branchService.GetBranchesAsync(branchId, take, filter);
        }

        public async Task<Result<IEnumerable<VBranch>>> GetBranchesByUser(string userId, int? branchId = 0)
        {
            return await _branchService.GetBranchesByUser(userId, branchId);
        }

        public async Task<Result<Branch>> GetBranchForVault(int? id)
        {
            return await _branchService.GetBranchForVault(id);
        }

        public async Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId)
        {
            return await _branchService.GetUpdateBranchById(branchId);
        }

        public async Task<Result> Update(Branch branch)
        {
            return await _branchService.Update(branch);
        }

        public async Task<Result> UpdateStatusAsync(int? branchId)
        {
            return await _branchService.UpdateStatusAsync(branchId);
        }


    }
}
