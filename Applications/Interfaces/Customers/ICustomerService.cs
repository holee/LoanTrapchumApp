using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Customers;

public interface ICustomerService
{
    Task<Result<string>> DeleteCustomerAsync(string? customerId,CancellationToken cancellation=default);
    Task<Result<string>> CreateAsync(EntityNames model, CancellationToken cancellation = default);
    Task<Result<string>> UpdateAsync(EntityNames model,CancellationToken cancellation = default);
    Task<Result<string>> AddCustomerNote(CustomerNote model);
    Task<Result<string>> AddCustomerActivity(CustomerActivity model);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId,bool includeInUsed = false);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes);
    Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId);
    Task<Pager<VCustomerList>> GetCustomersAsync(QueryParam param, CancellationToken cancellation = default);
    Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, bool include = false);
    Task<Result<IEnumerable<T>>> GetCustomerNotes<T>(string? customerId) where T : class;
    Task<Result<IEnumerable<T>>> GetCustomerActivies<T>(string? customerId) where T : class;
    Task<Result<Pager<T>>> GetCustomersAsync<T>(string? customerId, QueryParam param) where T : class;
    Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, bool state, CancellationToken cancellation = default);
    Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, bool include = false, CancellationToken cancellationToken = default);

    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(string? customerId);
    Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(int? branchId, string? customerId, bool includeComplete = false, CancellationToken cancellation = default);
}
