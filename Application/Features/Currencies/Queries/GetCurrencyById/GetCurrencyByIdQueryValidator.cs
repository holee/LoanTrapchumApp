namespace Application.Features.Currencies.Queries.GetCurrencyDetails;

public sealed class GetCurrencyByIdQueryValidator : AbstractValidator<GetCurrencyByIdQuery>
{
    public GetCurrencyByIdQueryValidator()
    { 
        RuleFor(x => x.CurrencyId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Currency Id is required.")
            .GreaterThan(0).WithMessage("Currency Id must be greater than zero.");
    }
}