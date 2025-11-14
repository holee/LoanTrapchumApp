using Applications.Features.Incomes.Dto;
using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Incomes.Command.CreateIncomeExpenseSource;

public record CreateIncomeExpenseCommand(CreateIncomeExpenseDto Request) : IRequest<ErrorOr<int>>; 
