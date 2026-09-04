namespace Application.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;

public sealed class GetDbAccountLoanSummaryQueryValidator : AbstractValidator<GetDbAccountLoanSummaryQuery>
{
    public GetDbAccountLoanSummaryQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .GreaterThan(0).When(x => x.BranchId.HasValue)
            .WithMessage("BranchId must be greater than 0.");
    }
}