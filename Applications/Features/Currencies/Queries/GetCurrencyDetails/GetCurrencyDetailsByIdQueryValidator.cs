namespace Applications.Features.Currencies.Queries.GetCurrencyDetails;

public sealed class GetCurrencyDetailsByIdQueryValidator : AbstractValidator<GetCurrencyDetailsByIdQuery>
{
    public GetCurrencyDetailsByIdQueryValidator()
    {
        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Currency Id is required.")
            .GreaterThan(0).WithMessage("Currency Id must be greater than zero.");
    }
}