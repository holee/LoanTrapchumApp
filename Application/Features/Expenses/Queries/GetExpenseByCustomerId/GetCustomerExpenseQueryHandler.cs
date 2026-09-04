using Application.Exceptions;
using Application.Interfaces.Customers;
using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;
using FluentValidation;

namespace Application.Features.Expenses.Queries.GetExpenseByCustomerId;

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
