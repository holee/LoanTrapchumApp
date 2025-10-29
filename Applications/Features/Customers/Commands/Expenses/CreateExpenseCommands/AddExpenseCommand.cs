using Applications.Features.Customers.Commands.Expenses.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.ExpenseCommands.CreateExpenseCommands;

public sealed record AddExpenseCommand(ExpenseDtoCreate Request) : IRequest<ErrorOr<long>>;
