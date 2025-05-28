using Applications.Commons.Mappers;
using Applications.Interfaces.Customers;
using Applications.Requests.Lists;
using Applications.Requests.Lists.Customers;
using Applications.Requests.Lists.OtherNames;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Lists.Customers;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Collaterals;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Loans;
using Domains.Models.Bases;
using Domains.Models.Lists.Customers;
using Mapster;
using System.Diagnostics;
using VCustomerList = Applications.Responses.ViewModels.Customers.VCustomerList;

namespace Applications.Services.Customers;

public interface ICustomerServiceImpl
{
    Task<Result<string>> AddCusomerNoteAsync(CustomerNoteDtoCreate model);
    Task<Result<string>> DeleteCustomerAsync(string? customerId);
    Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId);
    Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes);
    Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false);
    Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(string? customerId, bool include = false);
    Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(string? customerId);
    Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId);
    Task<Pager<VCustomerList>> GetCustomers(QueryParam param);

    Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(string? customerId, int take = 0,
        bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null);
    Task<Result<EntityNamesCreate>> GetCustomerUpdateAsync(string? customerId);
    Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, int editSeq = 1, bool isActive = true);
    Task<Result<string>> UpdateAsync(EntityNamesCreate customer);
    Task<Result<string>> CreateAsync(EntityNamesCreate customer);
    Task<Result<IEnumerable<VCustomerNotes>>> GetCusomerNotesAsync(string? customerId);
    Task<Result<IEnumerable<T>>> GetCusomerActivies<T>(string? customerId) where T : class;
    Task<Result<string>> AddCusomerActivyAsync(CustomerActivityDtoCreate model);
    Task<Result<Pager<T>>> GetCustomersAsync<T>(string? customerId, QueryParam param) where T : class;
    Task<Pager<T>> GetCustomerLoanRequestAsync<T>(int? branchId, string? customerId) where T : class;
}

public sealed class CustomerService(ICustomerService cusService) : ICustomerServiceImpl
{
    public async Task<Result<string>> AddCusomerNoteAsync(CustomerNoteDtoCreate model)
    {
        var req = model.Adapt<CustomerNote>();
        return await cusService.AddCustomerNote(req);
    }

    public async Task<Result<string>> AddCusomerActivyAsync(CustomerActivityDtoCreate model)
    {
        var req = model.Adapt<CustomerActivity>();

        return await cusService.AddCustomerActivity(req);
    }


    public async Task<Result<string>> UpdateAsync(EntityNamesCreate customer)
    {
        var req = customer.Adapt<EntityNames>();
        var cust = customer.ToCustomer();
        req.Customer = cust;
        return await cusService.UpdateAsync(req);
    }

    public async Task<Result<string>> CreateAsync(EntityNamesCreate entity)
    {
        entity.Id = $"{Guid.CreateVersion7()}";
        entity.EntType = EntType.Customer.ToString();
        var cust = entity.ToCustomer();
        var req = entity.ToEntityNames();
        req.Customer = cust;
        return await cusService.CreateAsync(req);
    }

    public async Task<Result<string>> DeleteCustomerAsync(string? customerId)
    {
        return await cusService.DeleteCustomerAsync(customerId);
    }

    public async Task<Result<IEnumerable<VCustomerNotes>>> GetCusomerNotesAsync(string? customerId)
    {
        return await cusService.GetCustomerNotes<VCustomerNotes>(customerId);
    }

    public async Task<Result<IEnumerable<T>>> GetCusomerActivies<T>(string? customerId)
        where T : class
    {
        return await cusService.GetCustomerActivies<T>(customerId);
    }


    public async Task<Result<Customer>> GetCustomerByIdAsync(string? customerId, params string[] includes)
    {
        return await cusService.GetCustomerByIdAsync(customerId, includes);
    }

    public async Task<Result<EntityNames>> GetCustomerByIdAsync(string? customerId)
    {
        return await cusService.GetCustomerByIdAsync(customerId);
    }

    public async Task<Result<IEnumerable<VCollateralLines>>> GetCustomerCollateralLines(string? customerId, bool includeInUsed = false)
    {
        return await cusService.GetCustomerCollateralLines(customerId, includeInUsed);
    }

    public async Task<Result<VCustomerList>> GetCustomerDetailsByIdAsync(string? customerId, bool include = false)
    {
        return await cusService.GetCustomerDetailByIdAsync(customerId, include);
    }


    public async Task<Pager<T>> GetCustomerLoanRequestAsync<T>(int? branchId, string? customerId)
        where T : class
    {
        var rsp = await cusService.GetCustomerLoanRequestAsync<T>(branchId, customerId);

        if (rsp.IsSuccess)
        {
            return new Pager<T>(rsp.Data.Items, rsp.Data.Count);
        }
        return new Pager<T>();
    }

    public async Task<Result<VCustomerList>> GetCustomerForRequestLoanAsync(string? customerId)
    {
        return await cusService.GetCustomerForRequestLoanAsync(customerId);
    }

    public async Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId)
    {
        return await cusService.GetCustomerLoanHistoryAsync(customerId);
    }

    public async Task<Pager<VCustomerList>> GetCustomers(QueryParam param)
    {
        return await cusService.GetCustomersAsync(param);
    }

    public Task<Result<IEnumerable<VCustomerList>>> GetCustomersAsync(string? customerId, int take = 0,
        bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Pager<T>>> GetCustomersAsync<T>(string? customerId, QueryParam param)
        where T : class
    {
        return await cusService.GetCustomersAsync<T>(customerId, param);
    }


    public async Task<Result<EntityNamesCreate>> GetCustomerUpdateAsync(string? customerId)
    {
        var updateCustomer = await cusService.GetCustomerByIdAsync(customerId);

        if (!updateCustomer.IsSuccess)
        {
            return Result.Failure<EntityNamesCreate>(ErrorResponse.NotFound(updateCustomer.ErrMessage));
        }

        var req = updateCustomer.Data?.ToCustomerCreate();

        if (req is null)
        {
            return Result.Failure<EntityNamesCreate>(ErrorResponse.NotFound("Cannot convert to entity name."));
        }


        return Result.Success(req!);
    }

    public async Task<Result<string>> MakeCustomerInActiveAsync(string? customerId, int editSeq = 0, bool isActive = true)
    {
        return await cusService.MakeCustomerInActiveAsync(customerId, editSeq, isActive);
    }

}