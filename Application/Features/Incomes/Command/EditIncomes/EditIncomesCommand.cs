using Application.Features.Incomes.Dto;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Incomes.Command.EditIncomes;

public sealed record EditIncomesCommand(string CustomerId, List<EditIncomeDto> Requests) : IRequest<ErrorOr<string>>;
 