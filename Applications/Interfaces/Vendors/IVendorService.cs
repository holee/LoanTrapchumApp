using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Interfaces.Vendors;

public interface IVendorService
{
    Task<ErrorOr<VendorVm>> CreateAsync(EntityNames vendor, CancellationToken cancellation = default);
    Task<ErrorOr<VendorVm>> UpdateAsync(EntityNames vendor, CancellationToken cancellation = default);
    Task<ErrorOr<Deleted>> DeleteAsync(string? vendorId, CancellationToken cancellation = default);
    Task<ErrorOr<Updated>> ChangeVendorStatusAsync(string? vendorId, bool state, CancellationToken cancellation = default);
    Task<ErrorOr<EntityNames>> GetVendorEntityByIdAsync(string? vendorId, CancellationToken cancellation = default);
    Task<ErrorOr<VendorVm>> GetVendorByIdAsync(string? vendorId, CancellationToken cancellation = default);
    Task<Pager<VendorVm>> GetVendorsAsync(QueryParam q, CancellationToken cancellation = default);
}
