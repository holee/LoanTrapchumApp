using FluentValidation;

namespace Applications.Features.Collaterals.Queries.GetCollateralsById;

public sealed class GetCollateralByIdQueryValidator : AbstractValidator<GetCollateralByIdQuery>
{
    public GetCollateralByIdQueryValidator()
    {
        RuleFor(x => x.CollateralId)
            .NotEmpty().WithMessage("Collateral ID is required.")
            .NotNull().WithMessage("Collateral ID cannot be null.");
    }
}
