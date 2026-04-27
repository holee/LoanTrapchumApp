using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Lists;

namespace Applications.Services.Lists
{
    public interface IVendorServiceImpl
    {
        Task<Result<string>> ChangeVendorStatus(string? vendorId,bool state,CancellationToken cancellation=default);
        Task<Result<string>> CreateAsync(EntityVendorCreate vendor);
        Task<Result<string>> UpdateAsync(EntityVendorCreate vendor);
        Task<Result<string>> DeleteAsync(string? vendorId);
        Task<Result<VendorDto>> GetVendorByIdAsync(string? vendorId);
        Task<Result<EntityNames>> GetVendorByIdAsync(string? vendorId, bool include);
        Task<Pager<VendorDto>> GetVendorsAsync(QueryParam q);
        Task<Result<EntityVendorCreate>> GetEditVendorAsync(string? vendorId);
    }
}
