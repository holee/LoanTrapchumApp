using Application.Commons.Mappers.Expenses;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.EditExpenseCommands;

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
