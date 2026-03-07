namespace Applications.Features.Dashbaord.Accountants.Queries.GetDbApprovedLoanRequest;

public sealed class GetDbApprovedRequestValidator : AbstractValidator<GetDbApprovedRequestQuery>
{
    public GetDbApprovedRequestValidator()
    {
        RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .When(x => x.BranchId.HasValue)
                .WithMessage("BranchId must be greater than 0.");
    }
}