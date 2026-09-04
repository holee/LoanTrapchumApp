namespace Application.Features.Loans.Queries.GetLateSchedules;

public sealed class GetLateSchedulesQueryValidator : AbstractValidator<GetLateSchedulesQuery>
{
    public GetLateSchedulesQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Please fill BranchID.")
                .GreaterThan(0)
                    .WithMessage("BranchId is must greater than 0.");
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
               .NotNull()
                    .WithMessage("Loan ID is required")
               .NotEmpty()
                    .WithMessage("Loan ID is required");
    }
}
