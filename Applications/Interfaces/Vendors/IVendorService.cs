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
        Task<Result<string>> CreateAsync(EntityNames vendor, CancellationToken cancellation = default);
        Task<Result<string>> UpdateAsync(EntityNames vendor, CancellationToken cancellation = default);
        Task<Result<string>> DeleteAsync(string? vendorId,CancellationToken cancellation = default);
        Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include);
        Task<Result<VendorVm>> GetVendorByIdAsync(string? vendorId);
        Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q, CancellationToken cancellation = default);
        Task<Result<string>> ChangeVendorStatus(string? vendorId, bool state, CancellationToken cancellation = default);
    }
}
