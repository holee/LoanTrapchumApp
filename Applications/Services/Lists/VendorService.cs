using Applications.Commons.Mappers;
using Applications.Interfaces.Vendors;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Bases;

namespace Applications.Services.LIsts
{
    public sealed class VendorService(IVendorService vService) : IVendorServiceImpl
    {

        public async Task<Result<string>> UpdateAsync(EntityVendorCreate vendor)
        {
            var model = vendor.ToVendorEdit();

            return await vService.UpdateAsync(model);
        }
        public async Task<Result<string>> CreateAsync(EntityVendorCreate vendor)
        {

            vendor.Id = $"{Guid.CreateVersion7()}";

            vendor.EntType = EntType.Vendor.ToString();

            var model = vendor.ToVendorCreate();

            return await vService.CreateAsync(model);

        }

        public async Task<Result<string>> DeleteAsync(string? vendorId)
        {
            return await vService.DeleteAsync(vendorId);
        }

        public async Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include)
        {
            return await vService.GetVendorByIdAsync(vendorId, include);
        }

        public async Task<Result<EntityVendorCreate>> GetEditVendorAsync(string? vendorId)
        {
            var rsp = await vService.GetVendorByIdAsync(vendorId, false);

            if (rsp.IsSuccess)
            {
                var model = rsp.Data!.ToVendor();

                return Result.Success(model);
            }

            return Result.Failure<EntityVendorCreate>(ErrorResponse.NotFound(Messages.DataNotFound));
        }


        public async Task<Result<VendorVm>> GetVendorByIdAsync(string? vendorId)
        {
            return await vService.GetVendorByIdAsync(vendorId);
        }

        public async Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q)
        {
            return await vService.GetVendorsAsync(q);
        }

        public async Task<Result<string>> ChangeVendorStatus(string? vendorId, bool state, CancellationToken cancellation = default)
        {
            return await vService.ChangeVendorStatus(vendorId, state, cancellation);
        }
    }
}
