using Application.Features.Loans.Dtos.Responses;
using Application.Responses.Paginations;
using Application.Responses.ViewModels.OtherNames;
using Domains.Models.Lists;
using Domains.Models.Lists.Customers;
using Domains.Models.Lists.Employees;
using Domains.Models.Lists.Vendors;
using ErrorOr;

namespace Application.Interfaces.Lists;

public interface IOtherNameService
{
    Task<ErrorOr<VOtherNames>> CreateAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<ErrorOr<VGuarantor>> CreateGaurantorAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<ErrorOr<VOtherNames>> UpdateAsync(EntityNames othername, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> DeleteAsync(string? othernameId, CancellationToken cancellation = default);
    Task<ErrorOr<VOtherNames>> UpdateStatusAsync(string? othernameId, CancellationToken cancellation = default);
    Task<ErrorOr<VOtherNames>> UpdateStatusAsync(string? othernameId, bool state, CancellationToken cancellation = default);
    Task<ErrorOr<EntityNames>> GetByIdAsync(string? id, CancellationToken cancellation = default);
    Task<ErrorOr<VOtherNames>> GetDetailAsync(string? id, CancellationToken cancellation = default);
    Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, string type, CancellationToken cancellation = default);
    Task<Pager<VOtherNames>> GetNamesAsync(QueryParam q, CancellationToken cancellation = default);
    Task<IEnumerable<VOtherNames>> GetNameListsAsync(string? nameId, QueryParam param, CancellationToken cancellation = default);
    Task<Pager<VOtherNames>> GetNameAsync(string? nameId, QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> ConvertToCustomerAsync(string? id, Customer customer, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> ConvertToVendorAsync(string? id, Vendor vendor, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> ConvertToEmployeeAsync(string? id, Employee employee, CancellationToken cancellation = default);
    Task<PageMetaData<VOtherNames>> GetNameListsAsync(string? nameId, QueryParam param, CancellationToken cancellation = default, params string[] columns);
}

