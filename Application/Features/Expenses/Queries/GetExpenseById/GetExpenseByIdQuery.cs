using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.Queries.GetExpenseById;

public sealed record GetExpenseByIdQuery(long Id,string CustomerId) : IRequest<ErrorOr<VCustomerExpenses>>;
