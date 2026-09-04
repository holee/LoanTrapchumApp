using Application.Features.Expenses.Dto;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.EditExpenseCommands; 

public sealed record EditExpenseCommand(ExpenseDtoUpdate Request) : IRequest<ErrorOr<long>>; 
