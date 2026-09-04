using Application.Features.LoanRequests.Requests;
using FluentValidation;

namespace Application.Features.LoanRequests.Command.VerifiedLoanRequest;

public sealed class VerifiedLoanRequestValidator : AbstractValidator<RequestDtoBaseCreate>
{
    public VerifiedLoanRequestValidator()
    {
        RuleFor(x => x.LoanRequestId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Loan Request Id cannot be null.")
            .NotEmpty().WithMessage("Loan Request Id is required.");
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Customer Id cannot be null.")
            .NotEmpty().WithMessage("Customer Id is required.");
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Branch Id cannot be null.")
            .GreaterThan(0).WithMessage("Branch Id must be greater than zero.");
    }
}
