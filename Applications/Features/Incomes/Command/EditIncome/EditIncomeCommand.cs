using Applications.Features.Incomes.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Command.EditIncome;

public sealed record EditIncomeCommand(EditIncomeDto Request) : IRequest<ErrorOr<long>>;
 