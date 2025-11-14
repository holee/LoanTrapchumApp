using Applications.Features.Incomes.Dto;
using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Incomes.Command.EditIncomeExpenseSource;

public record EditIncomeExpenseCommand(CreateIncomeExpenseDto Request) : IRequest<ErrorOr<int>>;
