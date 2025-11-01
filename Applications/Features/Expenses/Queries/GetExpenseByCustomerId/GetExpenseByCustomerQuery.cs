using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.Queries.GetExpenseByCustomerId;

public sealed record GetExpenseByCustomerQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerExpenses>>>;
