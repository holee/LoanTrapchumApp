using Application.Commons.Mappers.Expenses;
using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.CreateExpenseCommands;

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
