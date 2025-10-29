using Applications.Features.Customers.Commands.Incomes.Dto;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Customers.Commands.Incomes.EditIncome;

public sealed record EditIncomeCommand(EditIncomeDto Request) : IRequest<ErrorOr<long>>;
 