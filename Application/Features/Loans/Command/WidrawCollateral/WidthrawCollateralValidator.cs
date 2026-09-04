namespace Application.Features.Loans.Command.WidrawCollateral;

public sealed class WidthrawCollateralValidator : AbstractValidator<WidthrawCollateralDtoCreate>
{
    public WidthrawCollateralValidator()
    {
        RuleFor(x => x.LoanId)
                .NotNull()
                    .WithMessage("LoanId is required.")
                .NotEmpty()
                    .WithMessage("LoanId must be provided.");
        RuleFor(x => x.BranchId)
                .NotNull()
                    .WithMessage("BranchId is required.")
                .GreaterThan(0)
                    .WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.CollateralLineId)
                        .NotNull()
                            .WithMessage("CollateralLineId is required.")
                        .GreaterThan(0)
                            .WithMessage("CollateralLineId must be greater than 0.");
    }
}

