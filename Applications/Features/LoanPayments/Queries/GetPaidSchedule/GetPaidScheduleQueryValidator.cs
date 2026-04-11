namespace Applications.Features.LoanPayments.Queries.GetPaidSchedule;

public sealed class GetPaidScheduleQueryValidator : AbstractValidator<GetPaidScheduleQuery>
{
    public GetPaidScheduleQueryValidator()
    {
        RuleFor(x => x.BranchId)
            .NotNull()
                .WithMessage("BranchId is required.")
            .GreaterThan(0)
                .WithMessage("BranchId must be greater than 0.");

        RuleFor(x => x.LoanId)
            .NotNull()
                .WithMessage("LoanId is required.")
            .NotEmpty()
                .WithMessage("LoanId must not be empty.");
    }
}