using FluentValidation;

namespace Application.Features.Collaterals.Commands.DeleteCollateralLines;

public sealed class DeleteCollateralLineCommandValidator : AbstractValidator<DeleteCollateralLineCommand>
{
    public DeleteCollateralLineCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
        RuleFor(x => x.CollateralId)
            .NotEmpty().WithMessage("CollateralId is required.")
            .MaximumLength(50).WithMessage("CollateralId must not exceed 50 characters.");
    }
}

