using Applications.Features.Incomes.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Command.EditIncomes;

public sealed record EditIncomesCommand(string CustomerId, List<EditIncomeDto> Requests) : IRequest<ErrorOr<string>>;
 