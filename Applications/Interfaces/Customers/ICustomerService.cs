using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Loans;
using Domains.Models.Lists.Customers;

namespace Applications.Interfaces.Customers;

public interface ICustomerService
{
    Task<Result<Customer>> GetCustomerByIdAsync(long customerId); 
    Task<Result<string>> DeleteCustomerAsync(long customerId);
    Task<Result<string>> AddOrUpdateCustomerAsync(Customer customer);
    Task<Result<string>> UpdateCustomerAsync(Customer customer);
    Task<Result<string>> MakeCustomerInActiveAsync(long customerId,int editSeq=1,bool isActive=true);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(long customerId,bool includeInUsed = false);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(long customerId);
    Task<Result<Customer>> GetCustomerAsync(long customerId, params string[] includes);
    Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(long customerId=0,int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null);
    Task<Pager<VCustomerList>> GetCustomers(QueryParam param);
    Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(long customerId);
    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(long customerId);
    Task<Result<VCustomerList>> GetCustomerDetailsById(long customerId, bool include = false);
}
