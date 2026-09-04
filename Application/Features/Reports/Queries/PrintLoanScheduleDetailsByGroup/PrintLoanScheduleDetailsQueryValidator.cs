namespace Application.Features.Reports.Queries.PrintLoanScheduleDetailsByGroup;

public sealed class PrintLoanScheduleDetailsByGroupQueryValidator : AbstractValidator<PrintLoanScheduleDetailsByGroupQuery>
{
    public PrintLoanScheduleDetailsByGroupQueryValidator()
    {
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Loan ID is required.")
            .MaximumLength(250).WithMessage("Loan ID must not exceed 250 characters.");
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Branch ID is required.")
            .GreaterThan(0).WithMessage("Branch ID must be greater than 0.");
        RuleFor(x => x.ScheduleGroup).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Schedule Group is required.")
            .GreaterThan(0).WithMessage("Schedule Group must be greater than 0.");
    }
}