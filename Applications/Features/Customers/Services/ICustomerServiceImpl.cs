using Applications.Features.Customers.Requests;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Lists.Customers;
using System.Linq.Expressions;
namespace Applications.Features.Customers.Services;

public interface ICustomerServiceImpl
{
    Task<Result<string>> UpdateAsync(EntityCustomerCreate customer, CancellationToken cancellation = default);
    Task<Result<string>> CreateAsync(EntityCustomerCreate entity, CancellationToken cancellation = default);
    Task<Result<string>> DeleteCustomerAsync(string? customerId);
    Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false);
    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(string? customerId);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId);
    Task<Pager<VCustomerList>> GetCustomers(string? customerId, QueryParam param);
    Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(string? customerId, int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null);
    Task<Result<EntityCustomerCreate>> GetCustomerUpdateAsync(string? customerId);
    Task<Pager<VCustomerList>> GetCustomersAsync(string? customerId, QueryParam param);
    Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(int? branchId, string? customerId, bool include = false, CancellationToken cancellation = default);
    Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(string? customerId, bool include = false, CancellationToken cancellation = default);
    Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, params IEnumerable<Expression<Func<VCustomerList, object>>> includes);
}
