using Applications.Interfaces.Customers;
using Applications.Responses;
using Applications.Responses.ViewModels.Customers;

namespace Applications.Features.Customers.Commands.ExpenseCommands.Queries.GetExpenseByCustomerId;

public sealed class GetCustomerExpenseUseCase
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public GetCustomerExpenseUseCase(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<Result<IEnumerable<VCustomerExpenses>>> ExecuteAsync(string? customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {

            return Result.Failure<IEnumerable<VCustomerExpenses>>(ErrorResponse.Validation("Customrt ID IS Required."));
        }

        return await _custInomeSerivce.GetCustomerCustomerExpenses(customerId);
    }
}
