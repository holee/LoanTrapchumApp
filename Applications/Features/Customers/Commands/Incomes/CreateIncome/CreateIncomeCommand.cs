using Applications.Features.Customers.Commands.Incomes.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Incomes.CreateIncomeCommands;

public sealed record CreateIncomeCommand(CreateIncomeDto Request) : IRequest<ErrorOr<long>>;

