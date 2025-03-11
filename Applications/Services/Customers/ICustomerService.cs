using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Loans;
using Domains.Models.Lists.Customers;

namespace Applications.Services.Customers;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerService _cusService;

    public CustomerService(ICustomerService cusService)
    {
        _cusService = cusService;
    }

    public async Task<Result<string>> AddOrUpdateCustomerAsync(Customer customer)
    {
        return await _cusService.AddOrUpdateCustomerAsync(customer);
    }

    public async Task<Result<string>> DeleteCustomerAsync(long customerId)
    {
        return await _cusService.DeleteCustomerAsync(customerId);
    }

    public Task<Result<Customer>> GetCustomerAsync(long customerId, params string[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Customer>> GetCustomerByIdAsync(long customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(long customerId, bool includeInUsed = false)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VCustomerList>> GetCustomerDetailsById(long customerId, bool include = false)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(long customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(long customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(long customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Pager<VCustomerList>> GetCustomers(QueryParam param)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(long customerId = 0, int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> MakeCustomerInActiveAsync(long customerId, int editSeq = 1, bool isActive = true)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> UpdateCustomerAsync(Customer customer)
    {
        throw new NotImplementedException();
    }
}
