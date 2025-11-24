using FluentValidation;

namespace Applications.Features.LoanRequests.Command.ApplyLoanRequest;

public sealed class AppliedLoanRequestCommandValidator : AbstractValidator<AppliedLoanRequestCommand>
{
    public AppliedLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");
        RuleFor(x => x.Request.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than zero.");
        RuleFor(x => x.Request.LoanRequestId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("LoanRequestId is required.")
            .NotEmpty().WithMessage("LoanRequestId is required.");
    }
}