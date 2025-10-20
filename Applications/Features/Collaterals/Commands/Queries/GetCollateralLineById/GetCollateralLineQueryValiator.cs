using FluentValidation;

namespace Applications.Features.Collaterals.Commands.Queries.GetCollateralLineById
{
    public sealed class GetCollateralLineQueryValiator : AbstractValidator<GetCollateralLineDetailQuery>
    {
        public GetCollateralLineQueryValiator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id is required.");
            RuleFor(x => x.CollateralId).NotEmpty().WithMessage("CollateralId is required.");
        }
    }
}
