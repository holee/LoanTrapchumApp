using Applications.Commons.Mappers;
using Applications.Interfaces.Vendors;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Bases;
using Domains.Models.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.LIsts
{
    public sealed class VendorService(IVendorService vService) : IVendorServiceImpl
    {
        //public async Task<Result<string>> ChangeVendorStatus(EntVendorDtoChangeStatus req)
        //{
        //    return await vService.ChangeVendorStatus(req);
        //}
        public async Task<Result<string>> UpdateAsync(EntityNamesCreate vendor)
        {
            var model = vendor.ToEntityNames();

            model.Vendor = vendor.ToVendor();

            return await vService.UpdateAsync(model);
        }
        public async Task<Result<string>> CreateAsync(EntityNamesCreate vendor)
        {

            vendor.Id = $"{Guid.CreateVersion7()}";

            vendor.EntType = EntType.Vendor.ToString();

            var model = vendor.ToEntityNames();

            model.Vendor = vendor.ToVendor();

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

        public async Task<Result<EntityNamesCreate>> GetEditVendorAsync(string? vendorId)
        {
            var rsp = await vService.GetVendorByIdAsync(vendorId, false);

            if (rsp.IsSuccess)
            {
                var model = rsp.Data!.ToVendorCreate();

                return Result.Success(model);
            }

            return Result.Failure<EntityNamesCreate>(ErrorResponse.NotFound(Messages.DataNotFound));
        }


        public async Task<Result<VendorVm>> GetVendorByIdAsync(string? vendorId)
        {
            return await vService.GetVendorByIdAsync(vendorId);
        }

        public async Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q)
        {
            return await vService.GetVendorsAsync(q);
        }


    }
}
