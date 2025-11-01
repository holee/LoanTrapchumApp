using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Expenses.DeleteExpenseCommand;

public record DeleteExpenseCommand(long Id, string CustomerId) : IRequest<ErrorOr<string>>;