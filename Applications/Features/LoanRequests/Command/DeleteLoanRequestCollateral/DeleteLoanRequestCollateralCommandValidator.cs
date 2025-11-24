using FluentValidation;

namespace Applications.Features.LoanRequests.Command.DeleteLoanRequestCollateral;

public sealed class DeleteLoanRequestCollateralCommandValidator : AbstractValidator<DeleteLoanRequestCollateralCommand>
{
    public DeleteLoanRequestCollateralCommandValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.LoanRequestId)
            .NotEmpty().WithMessage("LoanRequestId is required.");
        RuleFor(x => x.CollateralLinkId)
            .NotEmpty().WithMessage("CollateralLinkId is required.")
            .GreaterThan(0).WithMessage("CollateralLinkId must be greater than 0.");
    }
}
