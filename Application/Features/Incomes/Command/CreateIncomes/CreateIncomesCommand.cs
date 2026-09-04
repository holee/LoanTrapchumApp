using Application.Features.Incomes.Dto;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.CreateIncomes;

public sealed record CreateIncomesCommand(string CustomerId, List<CreateIncomeDto> Request) : IRequest<ErrorOr<string>>;
