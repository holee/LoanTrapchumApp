namespace Application.Features.Loans.Command.WidrawCollateral;

public sealed class WidthrawCollateralValidators : AbstractValidator<WidrawalCollateralCommand>
{
    public WidthrawCollateralValidators()
    {
        RuleForEach(x => x.Request).SetValidator(new WidthrawCollateralValidator());
    }
}

