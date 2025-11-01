using FluentValidation;

namespace Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;

public sealed class AddOrUpdateCollateralCommandValidator : AbstractValidator<AddOrUpdateCollateralCommand>
{
    public AddOrUpdateCollateralCommandValidator()
    {
        RuleFor(x => x.Request.CustomerName)
                        .NotEmpty()
                            .WithMessage("Customer Is Required.");

        RuleFor(x => x.Request.CollateralLines)
                        .NotEmpty()
                            .WithMessage("At least one collateral line is required.");

        RuleForEach(x => x.Request.CollateralLines)
                .SetValidator(new CollateralLineCreateValidator());
    }
}