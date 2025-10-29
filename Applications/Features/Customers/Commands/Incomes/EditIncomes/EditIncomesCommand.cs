using Applications.Features.Customers.Commands.Incomes.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.IncomeCommands.EditIncomeCommands;

public sealed record EditIncomesCommand(string CustomerId, List<EditIncomeDto> Requests) : IRequest<ErrorOr<string>>;
 