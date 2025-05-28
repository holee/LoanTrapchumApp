using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Domains.Models.Lists.Customers;
namespace Applications.Interfaces.Customers;

public interface ICustomerService
{
    Task<Result<string>> DeleteCustomerAsync(string? customerId);
    Task<Result<string>> CreateAsync(EntityNames model);
    Task<Result<string>> UpdateAsync(EntityNames model);
    Task<Result<string>> AddCustomerNote(CustomerNote model);
    Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, int editSeq = 1, bool isActive = true);
    Task<Result<string>> AddCustomerActivity(CustomerActivity model);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId,bool includeInUsed = false);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes);
    Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId);
    Task<Pager<VCustomerList>> GetCustomersAsync(QueryParam param, CancellationToken cancellation = default);
    Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string? customerId, bool include = false);
    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(string? customerId);
    Task<Result<IEnumerable<T>>> GetCustomerNotes<T>(string? customerId) where T : class;
    Task<Result<IEnumerable<T>>> GetCustomerActivies<T>(string? customerId) where T : class;
    Task<Result<Pager<T>>> GetCustomersAsync<T>(string? customerId, QueryParam param) where T : class;
    Task<Result<Pager<T>>> GetCustomerLoanRequestAsync<T>(int? branchId, string? customerId,bool icludeComplete = false) where T : class;
}

//Task<Result<VCustomerList>> GetCustomerDetailByIdAsync(string customerId, bool include = false);
//Task<Result<IEnumerable<T>>> GetCustomersAsync<T>(string? customerId, int take = 0, bool includeInactive = false,
//    bool exceptActiveLoan = true, string? searchTerm = null)
//    where T : class;