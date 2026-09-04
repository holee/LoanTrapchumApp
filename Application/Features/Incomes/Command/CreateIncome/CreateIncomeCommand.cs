using Application.Features.Incomes.Dto;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.CreateIncome;

public sealed record CreateIncomeCommand(CreateIncomeDto Request) : IRequest<ErrorOr<long>>;

