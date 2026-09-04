using FluentValidation;

namespace Application.Features.LoanRequests.Command.AddLoanRequestCollateral;

public sealed class AddLoanRequestCollateralCommandValidator : AbstractValidator<AddLoanRequestCollateralCommand>
{
    public AddLoanRequestCollateralCommandValidator()
    {
        RuleFor(x => x.Request.LoanRequestId)
            .NotEmpty().WithMessage("Loan Request Id is required.");
        RuleFor(x => x.Request.BranchId)
            .NotEmpty().WithMessage("Branch Id is required.");
        RuleFor(x => x.Request.CustomerId)
            .NotEmpty().WithMessage("Customer Id is required.");

    }
}