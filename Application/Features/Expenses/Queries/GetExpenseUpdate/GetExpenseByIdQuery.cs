using Application.Features.Expenses.Dto;
using Application.Responses.ViewModels.Customers;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.Queries.GetExpenseById;

public  record GetExpenseByIdUpdateQuery(long Id,string CustomerId) : IRequest<ErrorOr<ExpenseDtoUpdate>>;
 