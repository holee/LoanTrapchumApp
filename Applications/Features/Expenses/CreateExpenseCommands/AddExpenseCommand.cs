using Applications.Features.Expenses.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.CreateExpenseCommands;

public sealed record AddExpenseCommand(ExpenseDtoCreate Request) : IRequest<ErrorOr<long>>;
