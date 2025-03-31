using Applications.Requests.Lists.Entities;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Lists;

namespace Applications.Services.LIsts
{
    public interface IVendorServiceImpl
    {
        Task<Result<string>> ChangeVendorStatus(EntVendorDtoChangeStatus req);
        Task<Result<string>> CreateAsync(EntNameDtoCreate vendor);
        Task<Result<string>> UpdateAsync(EntNameDtoUpdate vendor);
        Task<Result<string>> DeleteAsync(Guid? vendorId);
        Task<Result<VVendor>> GetVendorByIdAsync(Guid? vendorId);
        Task<Result<EntityName>> GetVendorByIdAsync(Guid? vendorId, bool include);
        Task<Pager<VVendor>> GetVendorsAsync(QueryParam q);
        Task<Result<EntNameDtoUpdate>> GetEditVendorAsync(Guid? vendorId);
    }
}
