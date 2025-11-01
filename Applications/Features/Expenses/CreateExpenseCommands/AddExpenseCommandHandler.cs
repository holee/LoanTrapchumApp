using Applications.Commons.Mappers.Expenses;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.CreateExpenseCommands;

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
