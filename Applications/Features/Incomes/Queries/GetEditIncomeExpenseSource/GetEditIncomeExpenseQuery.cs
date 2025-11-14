using Applications.Features.Incomes.Dto;
using Applications.Utils;
using ErrorOr;
namespace Applications.Features.Incomes.Command.GetEditIncomeExpenseSource; 

public record GetEditIncomeExpenseQuery(int? Id) : IRequest<ErrorOr<IncomeExpenseDto>>;
