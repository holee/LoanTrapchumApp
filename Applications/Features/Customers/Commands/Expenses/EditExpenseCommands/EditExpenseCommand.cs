using Applications.Features.Customers.Commands.ExpenseCommands.EditExpensCommands;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Expenses.EditExpenseCommands; 

public sealed record EditExpenseCommand(ExpenseDtoUpdate Request) : IRequest<ErrorOr<long>>; 
