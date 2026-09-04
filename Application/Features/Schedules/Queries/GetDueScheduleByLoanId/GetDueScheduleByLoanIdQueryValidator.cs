namespace Application.Features.Schedules.Queries.GetDueScheduleByLoanId;

public sealed class GetDueScheduleByLoanIdQueryValidator : AbstractValidator<GetDueScheduleByLoanIdQuery>
{
    public GetDueScheduleByLoanIdQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                                .NotNull().WithMessage("Branch ID is required")
                                .GreaterThan(0).WithMessage("Brach ID is greater than 0.");

        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("Loan ID is required")
                    .NotEmpty().WithMessage("Loan ID is required");
    }
}
