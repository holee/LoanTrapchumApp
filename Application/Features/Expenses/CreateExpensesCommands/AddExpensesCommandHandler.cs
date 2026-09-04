using Application.Commons.Mappers.Expenses;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.CreateExpensesCommands;

public sealed class AddExpensesCommandHandler : IRequestHandler<AddExpensesCommand, ErrorOr<string>>
{
    private readonly ICustomerIncomeExpendService _custExpSerivce;

    public AddExpensesCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custExpSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<string>> Handle(AddExpensesCommand request, CancellationToken cancellationToken = default)
    {

        var model = request.ToDtos();

        return await _custExpSerivce.AddOrUpdateExpensesAsync(request.CustomerId, model);
    }
}
