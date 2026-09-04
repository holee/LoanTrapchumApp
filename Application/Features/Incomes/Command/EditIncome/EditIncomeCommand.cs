using Application.Features.Incomes.Dto;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.EditIncome;

public sealed record EditIncomeCommand(EditIncomeDto Request) : IRequest<ErrorOr<long>>;
 