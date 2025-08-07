using Applications.Extensions.mappers;
using Applications.Interfaces.Customers;
using Applications.Requests.Lists.Entities;
using Applications.Responses;
using Domains.Models.Lists.Customers;
using Mapster;

namespace Applications.Features.Customers.UseCases;

public sealed class AddCustomerIncomeUseCase
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public AddCustomerIncomeUseCase(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<Result<string>> ExecuteAsync(string? customerId, List<CustomerIncomeDtoCreate> req)
    {

        var model = req.MapList<CustomerIncome>();

        return await _custInomeSerivce.AddIncomesAsync(customerId, model);

    }

    public async Task<Result<string>> ExecuteAsync(string? customerId, CustomerIncomeDtoCreate req)
    {

        var model = req.Adapt<CustomerIncome>();

        return await _custInomeSerivce.AddIncomesAsync(customerId, new List<CustomerIncome> { model });

    }
}
