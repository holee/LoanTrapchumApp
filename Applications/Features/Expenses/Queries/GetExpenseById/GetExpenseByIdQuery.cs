using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.Queries.GetExpenseById;

public sealed record GetExpenseByIdQuery(long Id,string CustomerId) : IRequest<ErrorOr<VCustomerExpenses>>;
