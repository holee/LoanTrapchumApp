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
        Task<Result> CreateAsync(Branch branch);
        Task<Result> DeleteAsync(int? id);
        Task<Result> UpdateAsync(BranchDtoUpdate branch);
        Task<Result<ResultStatus>> GetBrachStatus(int? branchRefId);
        Task<Result<Branch>> GetBranchById(int? id);
        Task<Pager<VBranchResponse>> GetBranchesAsync(QueryParam param);
        Task<Result<Pager<T>>> GetBranchesAsync<T>(int? branchId, QueryParam param) where T : class;
        Task<Result<IEnumerable<VBranchResponse>>> GetBranchesByUser(string? userId, int? branchId);
        Task<Result<UserBranchesResponse>> GetBranchesByUser(string userId);
        Task<Result<Branch>> GetBranchForVault(int? id);
        Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId);
        Task<Result> UpdateStatusAsync(int? branchId);
        Task<Result<T>> GetBranchById<T>(int? id) where T : class;
    }

    public sealed class BranchService(IBranchService branchService) : IBranchServiceImpl
    {
        public async Task<Result> CreateAsync(Branch branch)
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

        public async Task<Result<T>> GetBranchById<T>(int? id)
            where T : class
        {
            return await branchService.GetBranchById<T>(id);
        }

        public async Task<Result<Branch>> GetBranchById(int? id)
        {
            return await branchService.GetBranchById(id);
        }

        public async Task<Pager<VBranchResponse>> GetBranchesAsync(QueryParam param)
        {
            return await branchService.GetBranchesAsync(param);
        }

        public async Task<Result<Pager<T>>> GetBranchesAsync<T>(int? branchId, QueryParam param) where T : class
        {
            return await branchService.GetBranchesAsync<T>(branchId, param);
        }

        public async Task<Result<IEnumerable<VBranchResponse>>> GetBranchesByUser(string? userId, int? branchId)
        {
            return await branchService.GetBranchesByUser(userId, branchId);
        }

        public async Task<Result<UserBranchesResponse>> GetBranchesByUser(string userId) => await branchService.GetBranchesByUser(userId);


        public async Task<Result<Branch>> GetBranchForVault(int? id)
        {
            return await branchService.GetBranchForVault(id);
        }

        public async Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId)
        {
            return await branchService.GetUpdateBranchById(branchId);
        }

        public async Task<Result> UpdateAsync(BranchDtoUpdate branch)
        {

            var model = branch.Adapt<Branch>();

            return await branchService.UpdateAsync(model);
        }

        public async Task<Result> UpdateStatusAsync(int? branchId)
        {
            return await branchService.UpdateStatusAsync(branchId);
        }
    }
}