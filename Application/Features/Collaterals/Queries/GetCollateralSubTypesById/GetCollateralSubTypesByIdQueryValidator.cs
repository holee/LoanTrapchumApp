namespace Application.Features.Collaterals.Queries.GetCollateralSubTypesById;

public sealed class GetCollateralSubTypesByIdQueryValidator : AbstractValidator<GetCollateralSubTypesByIdQuery>
{
    public GetCollateralSubTypesByIdQueryValidator()
    {
        RuleFor(x => x.CollateralTypeId)
            .NotNull()
                .WithMessage("CollateralTypeId is required.")
            .GreaterThan(0)
                .WithMessage("CollateralTypeId must be greater than 0.");
    }
}
