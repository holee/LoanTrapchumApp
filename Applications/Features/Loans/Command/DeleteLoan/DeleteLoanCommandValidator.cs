namespace Applications.Features.Loans.Command.DeleteLoan;

public sealed class DeleteLoanCommandValidator : AbstractValidator<DeleteLoanCommand>
{
    public DeleteLoanCommandValidator()
    {
        RuleFor(x => x.LoanId)
            .NotEmpty().WithMessage("LoanId is required.")
            .NotNull().WithMessage("LoanId must be alphanumeric.");
        RuleFor(x => x.BranchId)
            .NotNull().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
    }
}
