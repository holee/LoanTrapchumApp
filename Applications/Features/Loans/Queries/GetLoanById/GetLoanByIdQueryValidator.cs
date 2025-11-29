using FluentValidation;

namespace Applications.Features.Loans.Queries.GetLoanById;

public sealed class GetLoanByIdQueryValidator : AbstractValidator<GetLoanByIdQuery>
{
    public GetLoanByIdQueryValidator() 
    {
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("LoanId cannot be null.")
            .NotEmpty().WithMessage("LoanId is required.");
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("BranchId cannot be null.")
            .GreaterThan(0).WithMessage("BranchId must be greater than zero.");
    }
}
