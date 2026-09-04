using ErrorOr;

namespace Application.Features.Incomes.Command.DeleteIncomeExpense
{
    public record DeleteIncomeExpenseCommand(int Id) : IRequest<ErrorOr<string>>;
}
