using Application.Features.Incomes.Dto;
using Application.Utils;
using ErrorOr;
namespace Application.Features.Incomes.Command.EditIncomeExpenseSource;

public record EditIncomeExpenseCommand(CreateIncomeExpenseDto Request) : IRequest<ErrorOr<int>>;
