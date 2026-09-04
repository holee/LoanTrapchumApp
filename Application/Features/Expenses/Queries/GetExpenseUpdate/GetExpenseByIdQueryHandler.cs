using Application.Features.Expenses.Dto;
using Application.Interfaces.Customers;
using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.Queries.GetExpenseById;

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