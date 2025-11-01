using Applications.Features.Expenses.EditExpenseCommands;
using FluentValidation;

namespace Applications.Features.Expenses.EditExpensesCommands;

public sealed class EditExpensesCommandValidattor : AbstractValidator<EditExpensesCommand>
{
    public EditExpensesCommandValidattor()
    {
        RuleFor(x => x.CustomerId).NotEmpty()
                                    .WithMessage("Customer ID is required.");

        // Validate that the Requests collection itself is not null or empty
        RuleFor(x => x.Requests).Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Expense update list is required.")
            .NotEmpty()
            .WithMessage("At least one expense item must be provided.");

        // Apply validator to each item in the Requests collection
        RuleForEach(x => x.Requests)
            .SetValidator(new ExpenseDtoUpdateValidator());

    }
}
