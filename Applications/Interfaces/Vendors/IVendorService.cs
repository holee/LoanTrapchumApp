using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Interfaces.Vendors;

public interface IVendorService
{
    Task<ErrorOr<VendorDto>> CreateAsync(EntityNames vendor, CancellationToken cancellation = default);
    Task<ErrorOr<VendorDto>> UpdateAsync(EntityNames vendor, CancellationToken cancellation = default);
    Task<ErrorOr<Deleted>> DeleteAsync(string? vendorId, CancellationToken cancellation = default);
    Task<ErrorOr<Updated>> ChangeVendorStatusAsync(string? vendorId, bool state, CancellationToken cancellation = default);
    Task<ErrorOr<EntityNames>> GetVendorEntityByIdAsync(string? vendorId, CancellationToken cancellation = default);
    Task<ErrorOr<VendorDto>> GetVendorByIdAsync(string? vendorId, CancellationToken cancellation = default);
    Task<Pager<VendorDto>> GetVendorsAsync(QueryParam q, CancellationToken cancellation = default);
    Task<ErrorOr<EntityVendorCreate>> GetEditVendorByIdAsync(string? vendorId, CancellationToken cancellation = default);
}
