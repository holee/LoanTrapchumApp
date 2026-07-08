namespace Applications.Features.Loans.Queries.GetLoanById;

public sealed class GetLoanByIdQueryValidator : AbstractValidator<GetLoanByIdQuery>
{
    public GetLoanByIdQueryValidator()
    {
        RuleFor(x => x.LoanId)
            .NotEmpty()
                .WithMessage("Loan ID is required.")
            .NotNull()
                .WithMessage("Loan ID must not be null.")
            .MaximumLength(450)
                .WithMessage("Loan ID must not exceed 450 characters.");
        RuleFor(x => x.BranchId)
            .NotNull()
                .WithMessage("Branch ID is required.")
            .GreaterThan(0)
                .WithMessage("Branch ID must be greater than 0.");
    }
}
