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
        Task<Result<string>> DeleteAsync(Guid? othernameId);
        Task<Result<string>> UpdateStatusAsync(Guid? othernameId);
        Task<Result<string>> ConvertToOtherNameAsync(Guid? id, EntityName othername);
        Task<Result<string>> ConvertToEmployeeAsync(Guid? id, Employee employee);
        Task<Result<string>> ConvertToCustomerAsync(Guid? id, Customer customer);
        Task<Result<string>> ConvertToVendorAsync(Guid? id, Vendor vendor);
        Task<EntityName?> GetByIdAsync(Guid? id);
        Task<EntityName?> GetDetailAsync(Guid? id);
        Task<IEnumerable<EntityName>> GetAllAsync(string? searchTerm, string sortField, string sortOrder, bool isActive = false);
        Task<Result<string>> CreateGaurantorAsync(EntityName othername);
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type);
    }
}
