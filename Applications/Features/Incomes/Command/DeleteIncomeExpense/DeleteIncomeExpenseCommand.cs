using ErrorOr;

namespace Applications.Features.Incomes.Command.DeleteIncomeExpense
{
    public record DeleteIncomeExpenseCommand(int Id) : IRequest<ErrorOr<string>>;
}
