using Applications.Features.Incomes.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Incomes.Command.CreateIncomes;

public sealed record CreateIncomesCommand(string CustomerId, List<CreateIncomeDto> Request) : IRequest<ErrorOr<string>>;
