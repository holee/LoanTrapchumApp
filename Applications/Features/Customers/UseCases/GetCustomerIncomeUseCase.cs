using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;

namespace Applications.Features.Customers.UseCases;


public sealed class GetCustomerIncomeUseCase
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public GetCustomerIncomeUseCase(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<Result<IEnumerable<VCustomerIncomes>>> ExecuteAsync(string? customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {

            return Result.Failure<IEnumerable<VCustomerIncomes>>(ErrorResponse.Validation("Customrt ID IS Required."));
        }

        return await _custInomeSerivce.GetCustomerCustomerIncomes(customerId);
    }
}
