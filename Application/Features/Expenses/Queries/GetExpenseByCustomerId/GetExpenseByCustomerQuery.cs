using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.Queries.GetExpenseByCustomerId;

public sealed record GetExpenseByCustomerQuery(string CustomerId) : IRequest<ErrorOr<IEnumerable<VCustomerExpenses>>>;
