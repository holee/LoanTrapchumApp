using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Lists.Customers;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Loans;
using Domains.Models.Lists.Customers;

namespace Applications.Services.Customers;

public interface ICustomerServiceImpl
{
    Task<Result<string>> AddOrUpdateCustomerAsync(Customer customer);
    Task<Result<string>> DeleteCustomerAsync(Guid? customerId);
    Task<Result<IEnumerable<CustomerNoteResponse>>> GetCusomerNotesAsyn(Guid? customerId, int v);
    Task<Result<Customer>> GetCustomerByIdAsync(Guid? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(Guid? customerId, params string[] includes);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(Guid? customerId, bool includeInUsed = false);
    Task<Result<VCustomerList>> GetCustomerDetailsById(Guid? customerId, bool include = false);
    Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(Guid? customerId);
    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(Guid? customerId);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(Guid? customerId);
    Task<Pager<VCustomerList>> GetCustomers(QueryParam param);
    Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(Guid? customerId, int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null);
    Task<Result<string>> MakeCustomerInActiveAsync(Guid? customerId, int editSeq = 1, bool isActive = true);
    Task<Result<string>> UpdateCustomerAsync(Customer customer);
}



public sealed class CustomerService(ICustomerService cusService) : ICustomerServiceImpl
{
    public async Task<Result<string>> AddOrUpdateCustomerAsync(Customer customer)
    {
        return await cusService.AddOrUpdateCustomerAsync(customer);
    }

    public async Task<Result<string>> DeleteCustomerAsync(Guid? customerId)
    {
        return await cusService.DeleteCustomerAsync(customerId);
    }

    public Task<Result<IEnumerable<CustomerNoteResponse>>> GetCusomerNotesAsyn(Guid? customerId, int v)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Customer>> GetCustomerByIdAsync(Guid? customerId, params string[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Customer>> GetCustomerByIdAsync(Guid? customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(Guid? customerId, bool includeInUsed = false)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VCustomerList>> GetCustomerDetailsById(Guid? customerId, bool include = false)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(Guid? customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(Guid? customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(Guid? customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Pager<VCustomerList>> GetCustomers(QueryParam param)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(Guid? customerId, int take = 0, bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> MakeCustomerInActiveAsync(Guid? customerId, int editSeq = 1, bool isActive = true)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> UpdateCustomerAsync(Customer customer)
    {
        throw new NotImplementedException();
    }
}
