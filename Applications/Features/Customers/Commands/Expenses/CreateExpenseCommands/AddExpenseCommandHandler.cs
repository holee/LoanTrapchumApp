using Applications.Features.Customers.Commands.Expenses.EditExpenseCommands;
using Applications.Features.Customers.Commands.Expenses.Mapper;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ExpenseCommands.CreateExpenseCommands;

public sealed class AddExpenseCommandHandler : IRequestHandler<AddExpenseCommand, ErrorOr<long>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public AddExpenseCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<long>> Handle(AddExpenseCommand request, CancellationToken cancellationToken = default)
    {

        var model = request.ToDto();

        return await _custInomeSerivce.AddOrUpdateExpenseAsync(model, cancellationToken);
    }
}
