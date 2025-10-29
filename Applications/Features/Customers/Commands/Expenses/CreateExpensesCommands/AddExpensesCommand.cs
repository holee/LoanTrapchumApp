using Applications.Features.Customers.Commands.Expenses.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Expenses.CreateExpensesCommands;

public sealed record AddExpensesCommand(string CustomerId, List<ExpenseDtoCreate> Requests) : IRequest<ErrorOr<string>>;
