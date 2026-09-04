using FluentValidation;

namespace Application.Features.Collaterals.Queries.GetCollateralLineDetails;

public sealed class GetCollateralLineDetailQueryValidator : AbstractValidator<GetCollateralLineDetailQuery>
{
    public GetCollateralLineDetailQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
        RuleFor(x => x.CollateralId)
            .NotEmpty().WithMessage("CollateralId is required.");
    }
}