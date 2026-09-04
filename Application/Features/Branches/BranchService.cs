using Application.Features.Branches.Dtos;
using Application.Features.Branches.Interfaces;
using Application.Interfaces.Branches;
using Application.Responses;
using Application.Responses.Paginations;
using Domains.Models.Lists;
using Mapster;

namespace Application.Features.Branches
{
    
    //public sealed class BranchService(IBranchService branchService) : IBranchServiceImpl
    //{
    //    public async Task<Result> CreateAsync(Branch branch)
    //    {
    //        return await branchService.CreateAsync(branch);
    //    }

    //    public async Task<Result> DeleteAsync(int? id)
    //    {
    //        return await branchService.DeleteAsync(id);
    //    }

    //    public async Task<Result<ResultStatus>> GetBrachStatus(int? branchRefId)
    //    {
    //        return await branchService.GetBrachStatus(branchRefId);
    //    }

    //    public async Task<Result<T>> GetBranchById<T>(int? id)
    //        where T : class
    //    {
    //        return await branchService.GetBranchById<T>(id);
    //    }

    //    public async Task<Result<Branch>> GetBranchById(int? id)
    //    {
    //        return await branchService.GetBranchById(id);
    //    }

    //    public async Task<Pager<VBranch>> GetBranchesAsync(QueryParam param)
    //    {
    //        return await branchService.GetBranchesAsync(param);
    //    }

    //    public async Task<Result<Pager<T>>> GetBranchesAsync<T>(int? branchId, QueryParam param) where T : class
    //    {
    //        return await branchService.GetBranchesAsync<T>(branchId, param);
    //    }

    //    public async Task<Result<IEnumerable<VBranch>>> GetBranchesByUser(string? userId, int? branchId)
    //    {
    //        return await branchService.GetBranchesByUser(userId, branchId);
    //    }

    //    public async Task<Result<UserBranchesResponse>> GetBranchesByUser(string? userId) => await branchService.GetBranchesByUser(userId);


    //    public async Task<Result<Branch>> GetBranchForVault(int? id)
    //    {
    //        return await branchService.GetBranchForVault(id);
    //    }

    //    public async Task<Result<BranchDtoUpdate>> GetUpdateBranchById(int? branchId)
    //    {
    //        return await branchService.GetUpdateBranchById(branchId);
    //    }

    //    public async Task<Result> UpdateAsync(BranchDtoUpdate branch)
    //    {

    //        var model = branch.Adapt<Branch>();

    //        return await branchService.UpdateAsync(model);
    //    }

    //    public async Task<Result> UpdateStatusAsync(int? branchId)
    //    {
    //        return await branchService.UpdateStatusAsync(branchId);
    //    }
    //    public async Task<Result<string>> UpdateStatusAsync(int? branchId,bool state)
    //    {
    //        return await branchService.UpdateStatusAsync(branchId,state);
    //    }
    //}

}