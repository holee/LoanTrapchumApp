using FluentValidation;

namespace Applications.Features.Expenses.EditExpensesCommands;

public sealed class ExpensesDtoUpdateValidator : AbstractValidator<ExpensesDtoUpdate>
{
    public ExpensesDtoUpdateValidator()
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
