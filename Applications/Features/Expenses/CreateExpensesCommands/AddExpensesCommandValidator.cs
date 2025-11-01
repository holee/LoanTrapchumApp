using FluentValidation;

namespace Applications.Features.Expenses.CreateExpensesCommands;

public sealed class AddExpensesCommandValidator : AbstractValidator<AddExpensesCommand>
{
    public AddExpensesCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");
        RuleFor(x => x.Requests)
            .NotEmpty().WithMessage("At least one expense is required.");
        RuleForEach(x => x.Requests).SetValidator(new ExpenseDtoCreateValidator());
    }
}
