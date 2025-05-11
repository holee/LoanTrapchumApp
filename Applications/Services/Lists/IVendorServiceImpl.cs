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
        //Task<Result<string>> ChangeVendorStatus(EntVendorDtoChangeStatus req);
        Task<Result<string>> CreateAsync(EntityNamesCreate vendor);
        Task<Result<string>> UpdateAsync(EntityNamesCreate vendor);
        Task<Result<string>> DeleteAsync(string? vendorId);
        Task<Result<VendorVm>> GetVendorByIdAsync(string? vendorId);
        Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include);
        Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q);
        Task<Result<EntityNamesCreate>> GetEditVendorAsync(string? vendorId);
    }
}
