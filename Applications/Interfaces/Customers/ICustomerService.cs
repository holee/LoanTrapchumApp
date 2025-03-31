using Applications.Requests.Lists.Customers;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Loans;
using Domains.Models.Lists.Customers;
using VCustomerList = Applications.Responses.ViewModels.Customers.VCustomerList;

namespace Applications.Interfaces.Customers;

public interface ICustomerService
{
    Task<Result<EntityNames>> GetCustomerByIdAsync(Guid? customerId);  
    Task<Result<string>> DeleteCustomerAsync(Guid? customerId);
    Task<Result<string>> MakeCustomerInActiveAsync(Guid? customerId,int editSeq=1,bool isActive=true);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(Guid? customerId,bool includeInUsed = false);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(Guid? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(Guid? customerId, params string[] includes);
    Task<Result<IEnumerable<T>>> GetCustomersAsync<T>(Guid? customerId,int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null)
        where T:class;
    Task<Pager<VCustomerList>> GetCustomers(QueryParam param);
    Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(Guid? customerId, bool include = false);
    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(Guid? customerId);
    Task<Result<string>> CreateAsync(EntityNames model);
    Task<Result<string>> UpdateAsync(EntityNames model);
    Task<Result<string>> AddCustomerNote(CustomerNote model); 
    Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string customerId, bool include = false);
    Task<Result<IEnumerable<T>>> GetCustomerNotes<T>(Guid? customerId) where T : class;
    Task<Result<IEnumerable<T>>> GetCustomerActivies<T>(Guid? customerId) where T : class;
    Task<Result<string>> AddCustomerActivity(CustomerActivity model);
}
