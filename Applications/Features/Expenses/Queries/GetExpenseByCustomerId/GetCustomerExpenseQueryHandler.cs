using Applications.Exceptions;
using Applications.Interfaces.Customers;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;
using FluentValidation;

namespace Applications.Features.Expenses.Queries.GetExpenseByCustomerId;

public sealed class GetCustomerExpenseQueryHandler : IRequestHandler<GetExpenseByCustomerQuery, ErrorOr<IEnumerable<VCustomerExpenses>>>
{
    private readonly ICustomerIncomeExpendService _customer;

    public GetCustomerExpenseQueryHandler(ICustomerIncomeExpendService customer)
    {
        _customer = customer;
    }

    public async Task<ErrorOr<IEnumerable<VCustomerExpenses>>> Handle(GetExpenseByCustomerQuery request, CancellationToken cancellation = default)
    {
        return await _customer.GetExpenses(request.CustomerId);
    }
}
