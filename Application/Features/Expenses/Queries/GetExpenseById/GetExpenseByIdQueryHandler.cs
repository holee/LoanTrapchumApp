using Application.Interfaces.Customers;
using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.Queries.GetExpenseById;

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