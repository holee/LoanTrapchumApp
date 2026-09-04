using Application.Features.Expenses.Dto;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.CreateExpenseCommands;

public sealed record AddExpenseCommand(ExpenseDtoCreate Request) : IRequest<ErrorOr<long>>;
