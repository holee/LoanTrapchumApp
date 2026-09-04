using Application.Features.Incomes.Dto;
using Application.Utils;
using ErrorOr;
namespace Application.Features.Incomes.Command.GetEditIncomeExpenseSource; 

public record GetEditIncomeExpenseQuery(int? Id) : IRequest<ErrorOr<IncomeExpenseDto>>;
