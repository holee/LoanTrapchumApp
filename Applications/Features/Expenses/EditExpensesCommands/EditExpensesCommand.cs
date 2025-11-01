using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.EditExpensesCommands;

public sealed record EditExpensesCommand(string CustomerId, List<ExpensesDtoUpdate> Requests) : IRequest<ErrorOr<string>>;
