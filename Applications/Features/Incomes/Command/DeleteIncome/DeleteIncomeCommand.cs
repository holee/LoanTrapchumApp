using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Command.DeleteIncome;

public record DeleteIncomeCommand(long Id, string? CustomerId) : IRequest<ErrorOr<bool>>;