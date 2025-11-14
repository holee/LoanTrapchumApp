using Applications.Features.Customers.Requests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Lists.Customers;
using System.Linq.Expressions;
using Applications.Features.OtherNames;
using Domains.Models.Lists;
using Applications.Features.Customers.Response;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Loans.Responses;

namespace Applications.Features.Customers.Services;

public interface ICustomerServiceImpl
{
    Task<Result<string>> UpdateAsync(EntityCustomerCreate customer, CancellationToken cancellation = default);
    Task<Result<string>> CreateAsync(EntityCustomerCreate entity, CancellationToken cancellation = default);
    Task<Result<string>> DeleteCustomerAsync(string? customerId); 
    Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false);
    Task<Result<VCustomerDetails>> GetCustomerForRequestLoanAsync(string? customerId);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId);
    Task<Result<IEnumerable<VCustomerDetails>>> GetCustomersAsync(string? customerId, int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null);
    Task<Result<EntityCustomerCreate>> GetCustomerUpdateAsync(string? customerId);
    Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(int? branchId, string? customerId, bool include = false, CancellationToken cancellation = default);
    Task<Result<VCustomerDetails>> GetCustomerDetailsByIdAsync(string? customerId, bool include = false, CancellationToken cancellation = default);

}
