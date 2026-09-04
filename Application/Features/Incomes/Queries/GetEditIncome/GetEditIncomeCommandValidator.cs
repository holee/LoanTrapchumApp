using FluentValidation;

namespace Application.Features.Incomes.Queries.GetEditIncome;

public sealed class GetEditIncomeCommandValidator : AbstractValidator<GetEditIncomeCommand>
{
    public GetEditIncomeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Income ID is required.");

        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Customer is required")
            .NotEmpty().WithMessage("Customer ID is required.");
    }
}
