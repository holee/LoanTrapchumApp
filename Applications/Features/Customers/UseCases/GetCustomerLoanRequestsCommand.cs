using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Customers.UseCases;

public sealed class GetCustomerLoanRequestsCommand
{
    private readonly ICustomerService _customerService;

    public GetCustomerLoanRequestsCommand(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    //public async Task<Result<IEnumerable<VLoanRequests>>> ExecuteAsync(string? customerId, bool onlyComplete = false, CancellationToken cancellation = default)
    //{

    //    if (string.IsNullOrWhiteSpace(customerId))
    //    {
    //        Result.Failure(ErrorResponse.Validation("Customer ID Is Required."));
    //    }

    //    return await _customerService.GetCustomerLoanRequestAsync(customerId,cancellation);

    //}


}
