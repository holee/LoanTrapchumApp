using Applications.Features.Collaterals.Commands.DeleteCOmmand;
using FluentValidation;

namespace Applications.Features.Collaterals.Commands.CollateralLineQuery
{
    public sealed class DeleteCollateralLineCommandValiator : AbstractValidator<DeleteCollateralLineCommand>
    {
        public DeleteCollateralLineCommandValiator() 
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id is required.");
            RuleFor(x => x.CollateralId).NotEmpty().WithMessage("CollateralId is required.");
        }
    }
}
