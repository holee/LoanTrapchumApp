using Applications.Commons.Mappers;
using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.Lists.Collaterals;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Domains.Models.Lists.Customers;
using System.Linq.Expressions;
using Applications.Features.OtherNames;
using Domains.Models.Lists;
using Applications.Features.Customers.Response;
using Applications.Features.Customers.Services;

namespace Applications.Features.Customers.UseCases;

public sealed class CustomerService(ICustomerService cusService) : ICustomerServiceImpl
{
    public async Task<Result<string>> UpdateAsync(EntityCustomerCreate customer, CancellationToken cancellation = default) 
    {

        var cust = customer.ToCustomerEdit();

        return await cusService.UpdateAsync(cust, cancellation);
    }

    public async Task<Result<string>> CreateAsync(EntityCustomerCreate entity, CancellationToken cancellation = default)
    {
      
        var cust = entity.ToCustomerCreate();

        return await cusService.CreateAsync(cust, cancellation);
    }

    public async Task<Result<string>> DeleteCustomerAsync(string? customerId)
    {
        return await cusService.DeleteCustomerAsync(customerId);
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

    public async Task<Result<VCustomerDetails>> GetCustomerDetailsByIdAsync(string? customerId, bool include = false)
    {
        return await cusService.GetCustomerDetailByIdAsync(customerId, include);
    }

    public async Task<Result<VCustomerDetails>> GetCustomerDetailsByIdAsync(string? customerId, bool include = false, CancellationToken cancellation = default)
    {
        return await cusService.GetCustomerDetailByIdAsync(customerId, include, cancellation);
    }



    public async Task<Result<VCustomerDetails>> GetCustomerForRequestLoanAsync(string? customerId)
    {
        return await cusService.GetCustomerForRequestLoanAsync(customerId);
    }

    public async Task<Result<IEnumerable<VLoanDisbursments>>> GetCustomerLoanHistoryAsync(string? customerId)
    {
        return await cusService.GetCustomerLoanHistoryAsync(customerId);
    }



    public Task<Result<IEnumerable<VCustomerDetails>>> GetCustomersAsync(string? customerId, int take = 0,
        bool includeInactive = false, bool exceptActiveLoan = true, string? searchTerm = null)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<EntityCustomerCreate>> GetCustomerUpdateAsync(string? customerId)
    {
        var updateCustomer = await cusService.GetCustomerByIdAsync(customerId);

        if (!updateCustomer.IsSuccess)
        {
            return Result.Failure<EntityCustomerCreate>(ErrorResponse.NotFound(updateCustomer.ErrMessage));
        }

        var req = updateCustomer.Data?.ToCustomer();

        if (req is null)
        {
            return Result.Failure<EntityCustomerCreate>(ErrorResponse.NotFound("Customer information was changed successfuly"));
        }

        return Result.Success(req!);
    }

    public async Task<Result<IEnumerable<VLoanRequests>>> GetCustomerLoanRequestAsync(int? branchId, string? customerId, bool include = false, CancellationToken cancellation = default)
    {
        return await cusService.GetCustomerLoanRequestAsync(customerId, include, cancellation);
    }

}