using Applications.Features.Incomes.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Command.CreateIncome;

public sealed record CreateIncomeCommand(CreateIncomeDto Request) : IRequest<ErrorOr<long>>;

