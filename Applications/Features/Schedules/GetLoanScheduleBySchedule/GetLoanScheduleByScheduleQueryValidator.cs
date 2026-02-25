namespace Applications.Features.Schedules.GetLoanScheduleBySchedule;

public sealed class GetLoanScheduleByScheduleQueryValidator : AbstractValidator<GetLoanScheduleByScheduleQuery>
{
    public GetLoanScheduleByScheduleQueryValidator() 
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("BranchId is required.")
                .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("LoanId is required.")
            .NotNull().WithMessage("LoanId is required.");
        RuleFor(x => x.ScheduleNum).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("ScheduleNum is required.")
            .GreaterThan(0).WithMessage("ScheduleNum must be greater than 0.");
    }
}
