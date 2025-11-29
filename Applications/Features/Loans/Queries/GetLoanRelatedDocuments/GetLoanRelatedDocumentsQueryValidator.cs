using FluentValidation;

namespace Applications.Features.Loans.Queries.GetLoanRelatedDocuments;

public sealed class GetLoanRelatedDocumentsQueryValidator : AbstractValidator<GetLoanRelatedDocumentsQuery>
{
    public GetLoanRelatedDocumentsQueryValidator()
    {
        RuleFor(x => x.LoanId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("LoanId cannot be null.")
            .NotEmpty().WithMessage("LoanId is required.");
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("BranchId cannot be null.")
            .GreaterThan(0).WithMessage("BranchId must be greater than zero.");
    }
}
