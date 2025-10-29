using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ExpenseCommands.Queries.GetExpenseByCustomerId;

public sealed class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ErrorOr<VCustomerExpenses>>
{
    private readonly ICustomerIncomeExpendService _customer;

    public GetExpenseByIdQueryHandler(ICustomerIncomeExpendService customer)
    {
        _customer = customer;
    }

    public async Task<ErrorOr<VCustomerExpenses>> Handle(GetExpenseByIdQuery request, CancellationToken cancellation = default)
    {
        return await _customer.GetExpense(request.Id, request.CustomerId);
    }

}