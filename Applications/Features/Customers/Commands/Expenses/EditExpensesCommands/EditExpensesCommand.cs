using Applications.Features.Customers.Commands.ExpenseCommands.EditExpensCommands;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ExpenseCommands.CreateExpenseCommands;

public sealed record EditExpensesCommand(string CustomerId, List<ExpensesDtoUpdate> Requests) : IRequest<ErrorOr<string>>;
