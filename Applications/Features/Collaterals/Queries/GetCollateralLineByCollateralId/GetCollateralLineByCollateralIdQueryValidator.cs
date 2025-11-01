using FluentValidation;

namespace Applications.Features.Collaterals.Queries.GetCollateralLineByCollateralId;

public sealed class GetCollateralLineByCollateralIdQueryValidator : AbstractValidator<GetCollateralLineByCollateralIdQuery>
{

    public GetCollateralLineByCollateralIdQueryValidator()
    {
        RuleFor(x => x.CollateralId).NotEmpty()
                                        .WithMessage("Collateral ID is required.");
    }

}
