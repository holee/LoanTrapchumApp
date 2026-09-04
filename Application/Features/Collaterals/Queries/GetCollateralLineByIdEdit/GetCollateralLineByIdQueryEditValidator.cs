using Application.Features.Collaterals.Queries.GetCollateralLineDetails;
using FluentValidation;

namespace Application.Features.Collaterals.Queries.GetCollateralLineById;

public sealed class GetCollateralLineByIdQueryEditValidator : AbstractValidator<GetCollateralLineDetailQuery>
{
    public GetCollateralLineByIdQueryEditValidator() 
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
        RuleFor(x => x.CollateralId)
            .NotEmpty().WithMessage("CollateralId is required.");
    }
}