using FluentValidation;

namespace Applications.Features.LoanRequests.Command.DeleteLoanRequestGuarantor;

public sealed class DeleteLoanRequestGuarantorCommandValidator : AbstractValidator<DeleteLoanRequestGuarantorCommand>
{
    public DeleteLoanRequestGuarantorCommandValidator()
    {
        RuleFor(x => x.LoanRequestId)
            .NotEmpty().WithMessage("Loan Request Id is required.")
            .MaximumLength(50).WithMessage("Loan Request Id must not exceed 50 characters.");
        RuleFor(x => x.BranchId)
            .NotNull().WithMessage("Branch Id is required.")
            .GreaterThan(0).WithMessage("Branch Id must be greater than 0.");
        RuleFor(x => x.GuarantorLinkId)
            .NotNull().WithMessage("Guarantor Link Id is required.")
            .GreaterThan(0).WithMessage("Guarantor Link Id must be greater than 0.");
    }
}