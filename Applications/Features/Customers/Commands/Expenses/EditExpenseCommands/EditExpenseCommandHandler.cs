using Applications.Features.Customers.Commands.Expenses.Mapper;
using Applications.Interfaces.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Expenses.EditExpenseCommands;

public sealed class EditExpenseCommandHandler : IRequestHandler<EditExpenseCommand, ErrorOr<long>>
{
    private readonly ICustomerIncomeExpendService _custInomeSerivce;

    public EditExpenseCommandHandler(ICustomerIncomeExpendService custInomeSerivce)
    {
        _custInomeSerivce = custInomeSerivce;
    }

    public async Task<ErrorOr<long>> Handle(EditExpenseCommand request, CancellationToken cancellationToken = default)
    {

        var model = request.ToDto();

        return await _custInomeSerivce.AddOrUpdateExpenseAsync(model, cancellationToken);
    }
}
