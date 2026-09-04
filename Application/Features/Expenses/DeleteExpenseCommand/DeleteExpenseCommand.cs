using Application.Utils;
using ErrorOr;

namespace Application.Features.Expenses.DeleteExpenseCommand;

public record DeleteExpenseCommand(long Id, string CustomerId) : IRequest<ErrorOr<string>>;