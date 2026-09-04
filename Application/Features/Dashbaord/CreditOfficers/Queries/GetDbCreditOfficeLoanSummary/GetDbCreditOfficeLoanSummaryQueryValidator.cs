namespace Application.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;

public sealed class GetDbCreditOfficeLoanSummaryQueryValidator : AbstractValidator<GetDbCreditOfficeLoanSummaryQuery>
{
    public GetDbCreditOfficeLoanSummaryQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("BranchId is required.")
                    .GreaterThan(0)
                    .WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.UserId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("UserId is required.")
                .NotEmpty().WithMessage("UserId is required.");
    }
}