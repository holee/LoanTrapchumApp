namespace Applications.Features.Loans.Queries.GetLoanByBranchId;

public sealed class GetLoanByBranchIdQueryValidator : AbstractValidator<GetLoanByBranchIdQuery>
{
    public GetLoanByBranchIdQueryValidator()
    {
        //RuleFor(x => x.BranchId)
        //            .GreaterThan(0)
        //                .WithMessage("PageIndex must be greater than or equal to 0.");
        RuleFor(x => x.State)
                 .IsInEnum()
                 .WithMessage("Invalid Loan Status.");
    }
}