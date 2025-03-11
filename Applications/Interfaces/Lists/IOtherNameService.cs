using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;

namespace Applications.Interfaces.Lists
{
    public interface IOtherNameService
    {
        Task<Result<string>> CreateAsync(EntityName othername);
        Task<Result<string>> DeleteAsync(long othernameId);
        Task<Result<string>> UpdateStatusAsync(long othernameId);
        Task<Result<string>> ConvertToOtherNameAsync(long id, EntityName othername);
        Task<Result<string>> ConvertToEmployeeAsync(long id, Employee employee);
        Task<Result<string>> ConvertToCustomerAsync(long id, Customer customer);
        Task<Result<string>> ConvertToVendorAsync(long id, Vendor vendor);
        Task<EntityName?> GetByIdAsync(long id);
        Task<EntityName?> GetDetailAsync(long id);
        Task<IEnumerable<EntityName>> GetAllAsync(string? searchTerm, string sortField, string sortOrder, bool isActive = false);
        Task<Result<string>> CreateGaurantorAsync(EntityName othername);
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type);
    }
}
