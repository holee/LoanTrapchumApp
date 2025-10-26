using FluentValidation;

namespace Applications.Features.Customers.Commands.IncomeCommands.EditIncomeCommands;

public sealed class SetIncomeUpdateValidator : AbstractValidator<IncomeDtoUpdate>
{
    public SetIncomeUpdateValidator()
    {
        RuleFor(x => x.IncomeSource)
            .NotEmpty().WithMessage("Income source is required.");

        RuleFor(x => x.Amount)
            .NotNull().GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .WithMessage("Description cannot exceed 200 characters.");
    }
}
