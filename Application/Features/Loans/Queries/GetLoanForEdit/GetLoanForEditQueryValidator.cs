namespace Application.Features.Loans.Queries.GetLoanForEdit;

public sealed class GetLoanForEditQueryValidator : AbstractValidator<GetLoanForEditQuery>
{
    public GetLoanForEditQueryValidator()
    {
        RuleFor(x => x.LoanId).NotEmpty().WithMessage("Loan ID is required.");

        RuleFor(x => x.BranchId).GreaterThan(0).WithMessage("Branch ID must be greater than 0.");
    }
}
