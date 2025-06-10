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
        Task<Result<string>> ChangeVendorStatus(string? vendorId,bool state,CancellationToken cancellation=default);
        Task<Result<string>> CreateAsync(EntityVendorCreate vendor);
        Task<Result<string>> UpdateAsync(EntityVendorCreate vendor);
        Task<Result<string>> DeleteAsync(string? vendorId);
        Task<Result<VendorVm>> GetVendorByIdAsync(string? vendorId);
        Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include);
        Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q);
        Task<Result<EntityVendorCreate>> GetEditVendorAsync(string? vendorId);
    }
}
