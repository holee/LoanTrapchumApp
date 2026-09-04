using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.EditExpensesCommands;

public sealed record EditExpensesCommand(string CustomerId, List<ExpensesDtoUpdate> Requests) : IRequest<ErrorOr<string>>;
