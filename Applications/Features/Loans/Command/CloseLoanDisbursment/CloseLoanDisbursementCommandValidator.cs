namespace Applications.Features.Loans.Command.CloseLoanDisbursment;

public sealed class CloseLoanDisbursementCommandValidator : AbstractValidator<CloseLoanDisbursementCommand>
{
    public CloseLoanDisbursementCommandValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("LoanId is required.")
            .NotEmpty().WithMessage("LoanId must not exceed 50 characters.");
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("CustomerId is required.")
            .NotEmpty().WithMessage("CustomerId must not exceed 50 characters.");
    }
}
