namespace Applications.Features.Schedules.Queries.GetScheduleById;

public sealed class GetScheduleByIdQueryValidator : AbstractValidator<GetScheduleByIdQuery>
{
    public GetScheduleByIdQueryValidator()
    {
        RuleFor(x => x.ScheduleId).NotNull().WithMessage("Schedule Id is required");
        RuleFor(x => x.BranchId).NotNull().WithMessage("Branch Id is required");
        RuleFor(x => x.LoanId).NotNull().WithMessage("Loan Id is required");
    }
}
