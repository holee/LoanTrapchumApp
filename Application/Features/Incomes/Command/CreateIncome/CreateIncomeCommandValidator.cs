using FluentValidation;

namespace Application.Features.Incomes.Command.CreateIncome;

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
            RuleFor(x => x.Request.Amount).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Amount is required.")
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");
            RuleFor(x => x.Request.IncomeSource)
                .NotEmpty()
                .WithMessage("Source is required.");
        });

    }
}

