using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Loans;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Customers;

public interface ICustomerService
{
    Task<Result<Customer>> GetCustomerByIdAsync(Guid? customerId); 
    Task<Result<string>> DeleteCustomerAsync(Guid? customerId);
    Task<Result<string>> AddOrUpdateCustomerAsync(Customer customer);
    Task<Result<string>> UpdateCustomerAsync(Customer customer);
    Task<Result<string>> MakeCustomerInActiveAsync(Guid? customerId,int editSeq=1,bool isActive=true);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(Guid? customerId,bool includeInUsed = false);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(Guid? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(Guid? customerId, params string[] includes);
    Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(Guid? customerId,int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null);
    Task<Pager<VCustomerList>> GetCustomers(QueryParam param);
    Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(Guid? customerId);
    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(Guid? customerId);
    Task<Result<VCustomerList>> GetCustomerDetailsById(Guid? customerId, bool include = false);
}
