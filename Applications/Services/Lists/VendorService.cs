using Applications.Commons.Mappers;
using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Interfaces.Vendors;
using Domains.Models.Bases;

namespace Applications.Services.Lists;

public sealed class VendorService(IVendorService vService) : IVendorServiceImpl
{
    public async Task<Result<string>> UpdateAsync(EntityVendorCreate vendor)
    {
        var model = vendor.ToVendorEdit();
        var result = await vService.UpdateAsync(model);
        return result.IsError
            ? Result.Failure<string>(ErrorResponse.Conflict(result.FirstError.Description))
            : Result.Success(result.Value.ToString()!);
    }

    public async Task<Result<string>> CreateAsync(EntityVendorCreate vendor)
    {
        vendor.Id = $"{Guid.CreateVersion7()}";
        vendor.EntType = EntType.Vendor.ToString();

        var model = vendor.ToVendorCreate();
        var result = await vService.CreateAsync(model);
        return result.IsError
            ? Result.Failure<string>(ErrorResponse.Conflict(result.FirstError.Description))
            : Result.Success(result.Value.ToString()!);
    }

    public async Task<Result<string>> DeleteAsync(string? vendorId)
    {
        var result = await vService.DeleteAsync(vendorId);
        return result.IsError
            ? Result.Failure<string>(ErrorResponse.NotFound(result.FirstError.Description))
            : Result.Success("Vendor deleted.");
    }

    public async Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include)
    {
        var result = await vService.GetVendorEntityByIdAsync(vendorId);
        return result.IsError
            ? Result.Failure<EntityNames>(ErrorResponse.NotFound(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Result<EntityVendorCreate>> GetEditVendorAsync(string? vendorId)
    {
        var result = await vService.GetVendorEntityByIdAsync(vendorId);
        if (result.IsError)
            return Result.Failure<EntityVendorCreate>(ErrorResponse.NotFound(result.FirstError.Description));

        var model = result.Value.ToEntityVendorCreate();
        return Result.Success(model);
    }

    public async Task<Result<VendorDto>> GetVendorByIdAsync(string? vendorId)
    {
        var result = await vService.GetVendorByIdAsync(vendorId);
        return result.IsError
            ? Result.Failure<VendorDto>(ErrorResponse.NotFound(result.FirstError.Description))
            : Result.Success(result.Value);
    }

    public async Task<Pager<VendorDto>> GetVendorsAsync(QueryParam q)
    {
        return await vService.GetVendorsAsync(q);
    }

    public async Task<Result<string>> ChangeVendorStatus(string? vendorId, bool state, CancellationToken cancellation = default)
    {
        var result = await vService.ChangeVendorStatusAsync(vendorId, state, cancellation);
        return result.IsError
            ? Result.Failure<string>(ErrorResponse.Conflict(result.FirstError.Description))
            : Result.Success("Vendor status changed.");
    }
}

