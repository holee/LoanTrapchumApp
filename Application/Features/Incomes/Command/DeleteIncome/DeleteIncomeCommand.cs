using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.DeleteIncome;

public record DeleteIncomeCommand(long Id, string? CustomerId) : IRequest<ErrorOr<bool>>;