using Applications.Extensions.mappers;
using Applications.Interfaces.Customers;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Domains.Models.Lists.Customers;
using Mapster;

namespace Applications.Features.Customers.UseCases;

public sealed class AddCustomerExpenseUseCase
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public AddCustomerExpenseUseCase(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<Result<string>> ExecuteAsync(string? customerId, List<CustomerExpenseDtoCreate> req)
    {

        var model = req.MapList<CustomerExpense>();

        return await _custInomeSerivce.AddExpensesAsync(customerId, model);

    }

    public async Task<Result<string>> ExecuteAsync(string? customerId, CustomerExpenseDtoCreate req)
    {

        var model = req.Adapt<CustomerExpense>();

        return await _custInomeSerivce.AddExpensesAsync(customerId, new List<CustomerExpense> { model });

    }
}
