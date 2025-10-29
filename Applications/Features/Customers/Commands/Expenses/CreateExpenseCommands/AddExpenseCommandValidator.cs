using Applications.Features.Customers.Commands.ExpenseCommands.CreateExpenseCommands;
using FluentValidation;

namespace Applications.Features.Customers.Commands.Expenses.CreateExpenseCommands;

public sealed class AddExpenseCommandValidator : AbstractValidator<AddExpenseCommand>
{
    public AddExpenseCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Expense request cannot be null.");

        RuleFor(x => x.Request.Amount)
            .GreaterThan(0)
            .WithMessage("Expense amount must be greater than zero.");

        RuleFor(x => x.Request.ExpenseTitle)
            .NotEmpty()
            .WithMessage("Expense title is required.");
    }
}
