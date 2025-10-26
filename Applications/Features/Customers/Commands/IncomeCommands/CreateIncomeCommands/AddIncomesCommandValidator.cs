using FluentValidation;

namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands;

public sealed class AddIncomesCommandValidator : AbstractValidator<AddIncomesCommand>
{
    public AddIncomesCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Incomes list is required.")
            .Must(incomes => incomes.Any())
            .WithMessage("At least one income item is required.");

        // Validate each income item
        RuleForEach(x => x.Request)
            .SetValidator(new SetIncomeCreateValidator());
    }

}
