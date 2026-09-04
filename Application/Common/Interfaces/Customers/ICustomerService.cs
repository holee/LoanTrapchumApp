using Application.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Application.Interfaces.Customers;

public interface ICustomerService
{

    Task<ErrorOr<VCustomerList>> UpdateAsync(EntityNames model, CancellationToken cancellation = default);
    Task<ErrorOr<VCustomerList>> CreateAsync(EntityNames model, CancellationToken cancellation = default);
    Task<DeleteResult> DeleteAsync(string? customerId, CancellationToken cancellation = default);
    Task<Pager<VCollateralLines>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false, CancellationToken cancellation = default);
    Task<EntityNames?> GetCustomerByIdAsync(string? customerId, CancellationToken cancellation = default);
    Task<Pager<VCustomerList>> GetCustomersAsync(int? branchId, string? userId, string? customerId, QueryParam param, bool isLoanActiveOnly = false, CancellationToken cancellation = default);
    Task<VCustomerDetails?> GetCustomerForRequestLoanAsync(string? customerId, CancellationToken cancellation = default);
    Task<ErrorOr<VCustomerDetails>> GetCustomerDetailByIdAsync(string? customerId, bool include = false, CancellationToken cancellationToken = default);
    Task<Pager<VLoanDisbursments>> GetCustomerLoanAsync(string? customerId, CancellationToken cancellation = default);
    Task<Pager<VLoanRequests>> GetCustomerLoanRequestAsync(string? customerId, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> MakeCustomerInActiveAsync(string? customerId, CancellationToken cancellation = default);
    Task<Pager<VLoanDisbursments>> GetCustomerLoanHistoryAsync(string? customerId, CancellationToken cancellation = default);
}
//Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId);
//Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes);
//Task<Pager<VCustomerList>> GetCustomersAsync(int? branchId, string? userId, string? customerId, QueryParam param, CancellationToken cancellation = default);
//Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(string? customerId, bool includeComplete = false, CancellationToken cancellation = default);
//Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, bool state, CancellationToken cancellation = default);
//Task<Result<string>> CreateAsync(EntityNames model, CancellationToken cancellation = default);
//Task<Result<string>> UpdateAsync(EntityNames model, CancellationToken cancellation = default);
//Task<Result<string>> DeleteCustomerAsync(string? customerId, CancellationToken cancellation = default);
