namespace Applications.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;

public sealed class GetLoanSchedulesMonthToDateQueryValidator : AbstractValidator<GetLoanSchedulesMonthToDateQuery>
{
    public GetLoanSchedulesMonthToDateQueryValidator()
    {
        RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .WithMessage("Brach ID is required.");
    }
}