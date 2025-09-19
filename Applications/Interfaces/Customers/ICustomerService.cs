using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Lists.Customers;
using System.Linq.Expressions;
using Domains.Models.Lists;

namespace Applications.Interfaces.Customers;

public interface ICustomerService
{
    Task<Result<string>> DeleteCustomerAsync(string? customerId, CancellationToken cancellation = default);
    Task<Result<string>> CreateAsync(EntityNames model, CancellationToken cancellation = default);
    Task<Result<string>> UpdateAsync(EntityNames model, CancellationToken cancellation = default);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes); 
    Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId);
    Task<Pager<VCustomerList>> GetCustomersAsync(string? customerId, QueryParam param, CancellationToken cancellation = default);
    Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, bool state, CancellationToken cancellation = default);
    Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, bool include = false, CancellationToken cancellationToken = default);
    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(string? customerId, CancellationToken cancellation = default);
    Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(string? customerId, bool includeComplete = false, CancellationToken cancellation = default);
    Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, params IEnumerable<Expression<Func<VCustomerList, object>>> includes);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanAsync(string? customerId, CancellationToken cancellation = default);
}
