using Applications.Features.Customers.Commands.Expenses.EditExpenseCommands;
using FluentValidation;

namespace Applications.Features.Customers.Commands.ExpenseCommands.CreateExpenseCommands;

public sealed class EditExpenseCommandValidattor : AbstractValidator<EditExpenseCommand>
{
    public EditExpenseCommandValidattor()
    {
        // Validate that the Requests collection itself is not null or empty
        RuleFor(x => x.Request).Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Expense update list is required.")
            .NotEmpty()
            .WithMessage("At least one expense item must be provided.");

        // Apply validator to each item in the Requests collection
        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.Id)
                            .GreaterThan(0)
                            .WithMessage("Expense ID must be greater than zero.");

            RuleFor(x => x.Request.Amount)
                            .GreaterThan(0)
                            .WithMessage("Expense amount must be greater than zero.");

            RuleFor(x => x.Request.ExpenseTitle)
                            .NotEmpty()
                            .MaximumLength(200)
                            .WithMessage("Title is required.");
        });

    }
}
