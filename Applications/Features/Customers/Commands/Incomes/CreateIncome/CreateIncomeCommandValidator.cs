using Applications.Features.Customers.Commands.Incomes.CreateIncomeCommands;
using FluentValidation;

namespace Applications.Features.Customers.Commands.IncomeCommands.CreateIncomeCommands;

public sealed class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
{
    public CreateIncomeCommandValidator()
    {
        RuleFor(x => x.Request.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId is required.");

        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("IncomeDtoCreate is required.");

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");
            RuleFor(x => x.Request.IncomeSource)
                .NotEmpty()
                .WithMessage("Source is required.");
        });

    }
}

