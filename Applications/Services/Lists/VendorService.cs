using Applications.Interfaces.Vendors;
using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
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
        public async Task<Result<string>> ChangeVendorStatus(EntVendorDtoChangeStatus req)
        {
            return await vService.ChangeVendorStatus(req);
        }

        public Task<Result<string>> UpdateAsync(EntNameDtoCreate vendor)
        {
            throw new NotImplementedException();
        }
        public Task<Result<string>> CreateAsync(EntNameDtoCreate vendor)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> DeleteAsync(Guid? vendorId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<EntityName>> GetVendorByIdAsync(Guid? vendorId, bool include)
        {
            throw new NotImplementedException();
        }

        public Task<Result<EntNameDtoUpdate>> GetEditVendorAsync(Guid? vendorId)
        {
            throw new NotImplementedException();
        }


        public Task<Result<VVendor>> GetVendorByIdAsync(Guid? vendorId)
        {
            throw new NotImplementedException();
        }

        public Task<Pager<VVendor>> GetVendorsAsync(QueryParam q)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> UpdateAsync(EntNameDtoUpdate vendor)
        {
            throw new NotImplementedException();
        }
    }
}
