using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Incomes.DeleteIncome;

public record DeleteIncomeCommand(long Id, string? CustomerId) : IRequest<ErrorOr<bool>>;