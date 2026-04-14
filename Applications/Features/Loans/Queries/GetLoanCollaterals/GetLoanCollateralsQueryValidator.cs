namespace Applications.Features.Loans.Queries.GetLoanCollaterals;

public sealed class GetLoanCollateralsQueryValidator : AbstractValidator<GetLoanCollateralsQuery>
{
    public GetLoanCollateralsQueryValidator()
    {
        RuleFor(x => x.LoanId)
                .NotNull()
                    .WithMessage("LoanId cannot be null.")
                .NotEmpty()
                    .WithMessage("LoanId is required.");
        RuleFor(x => x.BranchId)
                .NotNull()
                    .WithMessage("BranchId cannot be null.")
                .GreaterThan(0)
                    .WithMessage("BranchId must be greater than 0.");
    }
}