namespace Application.Features.Reports.Queries.PrintLoanScheduleDetails;

public sealed class PrintLoanScheduleDetailsQueryValidator : AbstractValidator<PrintLoanScheduleDetailsQuery>
{
    public PrintLoanScheduleDetailsQueryValidator()
    {
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Loan ID is required.")
            .MaximumLength(250).WithMessage("Loan ID must not exceed 250 characters.");
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Branch ID is required.")
            .GreaterThan(0).WithMessage("Branch ID must be greater than 0.");
    }
}