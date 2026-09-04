using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.EditExpensesCommands;

public sealed record EditExpensesCommandHandler : IRequestHandler<EditExpensesCommand, ErrorOr<string>>
{
    private readonly ICustomerIncomeExpendService _custExpendService;

    public EditExpensesCommandHandler(ICustomerIncomeExpendService custIncomeExpendService)
    {
        _custExpendService = custIncomeExpendService;
    }

    public async Task<ErrorOr<string>> Handle(EditExpensesCommand request, CancellationToken cancellationToken = default)
    {

        var models = request.ToDtos();

        return await _custExpendService.AddOrUpdateExpensesAsync(request.CustomerId, models);

    }
}
