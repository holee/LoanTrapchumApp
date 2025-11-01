using Applications.Features.Expenses.Dto;
using Applications.Responses.ViewModels.Customers;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.Queries.GetExpenseById;

public sealed record GetExpenseByIdUpdateQuery(long Id,string CustomerId) : IRequest<ErrorOr<ExpenseDtoUpdate>>;
 