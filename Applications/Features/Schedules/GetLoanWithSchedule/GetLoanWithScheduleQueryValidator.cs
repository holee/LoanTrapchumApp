namespace Applications.Features.Schedules.GetLoanWithSchedule;

public sealed class GetLoanWithScheduleQueryValidator : AbstractValidator<GetLoanWithScheduleQuery>
{
    public GetLoanWithScheduleQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("BranchId is required.")
                .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("LoanId is required.")
            .NotNull().WithMessage("LoanId is required.");
    }
}
