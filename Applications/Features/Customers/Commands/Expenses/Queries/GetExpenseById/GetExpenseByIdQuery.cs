using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ExpenseCommands.Queries.GetExpenseByCustomerId;

public sealed record GetExpenseByIdQuery(long Id,string CustomerId) : IRequest<ErrorOr<VCustomerExpenses>>;
