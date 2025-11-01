using Applications.Features.Expenses.Dto;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.Queries.GetExpenseById;

public sealed class GetExpenseByIdUpdateQueryHandler : IRequestHandler<GetExpenseByIdUpdateQuery, ErrorOr<ExpenseDtoUpdate>>
{
    private readonly ICustomerIncomeExpendService _customer;

    public GetExpenseByIdUpdateQueryHandler(ICustomerIncomeExpendService customer)
    {
        _customer = customer;
    }

    public async Task<ErrorOr<ExpenseDtoUpdate>> Handle(GetExpenseByIdUpdateQuery request, CancellationToken cancellation = default)
    {
        return await _customer.GetEditExpensesAsync(request.Id, request.CustomerId, cancellation);
    }

}