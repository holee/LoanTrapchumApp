using Applications.Features.Expenses.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.EditExpenseCommands; 

public sealed record EditExpenseCommand(ExpenseDtoUpdate Request) : IRequest<ErrorOr<long>>; 
