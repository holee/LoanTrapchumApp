using Applications.Features.Expenses.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.CreateExpensesCommands;

public sealed record AddExpensesCommand(string CustomerId, List<ExpenseDtoCreate> Requests) : IRequest<ErrorOr<string>>;
