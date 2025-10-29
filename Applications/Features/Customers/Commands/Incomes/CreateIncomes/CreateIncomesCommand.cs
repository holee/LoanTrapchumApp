using Applications.Features.Customers.Commands.Incomes.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Incomes.CreateIncomes;

public sealed record CreateIncomesCommand(string CustomerId, List<CreateIncomeDto> Request) : IRequest<ErrorOr<string>>;
