using Application.Features.Incomes.Dto;
using Application.Utils;
using ErrorOr;
namespace Application.Features.Incomes.Command.CreateIncomeExpenseSource;

public record CreateIncomeExpenseCommand(CreateIncomeExpenseDto Request) : IRequest<ErrorOr<int>>; 
