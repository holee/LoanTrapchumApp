using Application.Interfaces.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.DeleteExpenseCommand;

public sealed class DeleteExpenseCommandHandler(ICustomerIncomeExpendService expendService)
    : IRequestHandler<DeleteExpenseCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteExpenseCommand request,
        CancellationToken cancellationToken = default)
    {
        return await expendService.DeleteExpensesAsync(request.Id, request.CustomerId, cancellationToken);
    }
}