namespace Applications.Features.Customers.Services;

public interface ICustomerServiceImpl
{
   
    //Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false);
    Task<Result<IEnumerable<VCustomerDetails>>> GetCustomersAsync(string? customerId, int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null);
    //Task<Result<EntityCustomerCreate>> GetCustomerUpdateAsync(string? customerId);
  
}
//Task<Result<VCustomerDetails>> GetCustomerForRequestLoanAsync(string? customerId);

//Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(int? branchId, string? customerId, bool include = false, CancellationToken cancellation = default);
//Task<Result<VCustomerDetails>> GetCustomerDetailsByIdAsync(string? customerId, bool include = false, CancellationToken cancellation = default);
//Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId);
//Task<Result<string>> DeleteCustomerAsync(string? customerId);
//Task<Result<string>> UpdateAsync(EntityCustomerCreate customer, CancellationToken cancellation = default);
//Task<Result<string>> CreateAsync(EntityCustomerCreate entity, CancellationToken cancellation = default);
//Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes);
//Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId);
