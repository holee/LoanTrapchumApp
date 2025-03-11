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
        Task<Result<string>> DeleteAsync(long vendorId);
        Task<Result<EntityName>> GetVendorByIdAsync(long vendorId, bool include);
        Task<Result<VVendor>> GetVendorByIdAsync(long vendorId);
        Task<Pager<VVendor>> GetVendorsAsync(QueryParam q);
    }
}
