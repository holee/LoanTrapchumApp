using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Lists;

namespace Applications.Interfaces.Vendors
{
    public interface IVendorService
    {
        //Task<Result<string>> ChangeVendorStatus(EntVendorDtoChangeStatus req);
        Task<Result<string>> CreateAsync(EntityNames vendor);
        Task<Result<string>> UpdateAsync(EntityNames vendor); 
        Task<Result<string>> DeleteAsync(string? vendorId);
        Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include);
        Task<Result<VendorVm>> GetVendorByIdAsync(string? vendorId);
        Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q);
    }
}
