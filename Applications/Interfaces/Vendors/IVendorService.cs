using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Lists;

namespace Applications.Interfaces.Vendors
{
    public interface IVendorService
    {
        Task<Result<string>> ChangeVendorStatus(EntVendorDtoChangeStatus req);
        Task<Result<string>> CreateOrUpdate(EntityName vendor);
        Task<Result<string>> DeleteAsync(Guid? vendorId);
        Task<Result<EntityName>> GetVendorByIdAsync(Guid? vendorId, bool include);
        Task<Result<VVendor>> GetVendorByIdAsync(Guid? vendorId);
        Task<Pager<VVendor>> GetVendorsAsync(QueryParam q);
    }
}
