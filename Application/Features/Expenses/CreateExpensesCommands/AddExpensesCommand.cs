using Application.Features.Expenses.Dto;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.CreateExpensesCommands;

public sealed record AddExpensesCommand(string CustomerId, List<ExpenseDtoCreate> Requests) : IRequest<ErrorOr<string>>;
