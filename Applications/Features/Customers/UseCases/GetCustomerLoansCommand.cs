using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans;

namespace Applications.Features.Customers.UseCases;

public sealed class GetCustomerLoansCommand
{
    private readonly ICustomerService _customerService;

    public GetCustomerLoansCommand(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Result<IEnumerable<VLoanDisbursments>>> ExecuteAsync(string? customerId, bool onlyComplete = false, CancellationToken cancellation = default)
    {

        if (string.IsNullOrWhiteSpace(customerId))
        {
            Result.Failure(ErrorResponse.Validation("Customer ID Is Required."));
        }

        return await _customerService.GetCustomerLoanAsync(customerId, cancellation);

    }


}
