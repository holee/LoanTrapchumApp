using FluentValidation;

namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands;

public sealed class SetIncomeCreateValidator : AbstractValidator<IncomeDtoCreate>
{
    public SetIncomeCreateValidator()  
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