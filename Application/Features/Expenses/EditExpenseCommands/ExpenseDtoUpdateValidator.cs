using Application.Features.Expenses.EditExpensesCommands;
using FluentValidation;

namespace Application.Features.Expenses.EditExpenseCommands;

public sealed class ExpenseDtoUpdateValidator : AbstractValidator<ExpensesDtoUpdate>
{
    public ExpenseDtoUpdateValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Expense ID must be greater than zero.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Expense amount must be greater than zero.");

        RuleFor(x => x.ExpenseTitle)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Title is required.");

    }
}
