using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.OtherNames;
using Domains.Models.Bases;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;

namespace Applications.Interfaces.Lists
{
    public interface IOtherNameService
    {
        Task<Result<string>> CreateAsync(EntityNames othername);
        Task<Result<string>> DeleteAsync(string? othernameId);
        Task<Result<string>> UpdateStatusAsync(string? othernameId);
        Task<Result<string>> ConvertToEmployeeAsync(string? id, Employee employee, CancellationToken cancellation = default);
        Task<Result<string>> ConvertToCustomerAsync(string? id, Customer customer, CancellationToken cancellation = default);
        Task<Result<string>> ConvertToVendorAsync(string? id, Vendor vendor, CancellationToken cancellation = default);
        Task<Result<EntityNames>> GetByIdAsync(string? id);
        Task<Result<string>> CreateGaurantorAsync(EntityNames othername);
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type);
        Task<Result<T>> GetDetailAsync<T>(string? id) where T : class;
        Task<Result<string>> UpdateAsync(EntityNames othername);
        Task<IEnumerable<T>> GetNameLists<T>(string? nameId, QueryParam param) where T : class, new();
        Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, CancellationToken cancellation = default);
        Task<Result<VOtherNames>> UpdateStatusAsync(string? othernameId, bool state, CancellationToken cancellation = default);
    }
}
