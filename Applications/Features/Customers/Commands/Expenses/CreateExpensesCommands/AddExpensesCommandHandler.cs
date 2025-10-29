using Applications.Features.Customers.Commands.Expenses.CreateExpensesCommands;
using Applications.Features.Customers.Commands.Expenses.Mapper;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ExpenseCommands.CreateExpenseCommands;

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
