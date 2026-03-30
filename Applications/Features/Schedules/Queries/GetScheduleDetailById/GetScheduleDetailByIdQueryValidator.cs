namespace Applications.Features.Schedules.Queries.GetScheduleDetailById;

public sealed class GetScheduleDetailByIdQueryValidator : AbstractValidator<GetScheduleDetailByIdQuery>
{
    public GetScheduleDetailByIdQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("BranchId is required.")
                .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("LoanId is required.")
            .NotEmpty().WithMessage("LoanId cannot be empty.");
        RuleFor(x => x.ScheduleId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("ScheduleId is required.")
            .GreaterThan(0).WithMessage("ScheduleId must be greater than 0.");
    }
}
