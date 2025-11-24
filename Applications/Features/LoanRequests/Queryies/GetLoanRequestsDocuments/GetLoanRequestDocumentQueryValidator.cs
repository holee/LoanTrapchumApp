using FluentValidation;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;

public sealed class GetLoanRequestDocumentQueryValidator : AbstractValidator<GetLoanRequestDocumentQuery>
{
    public GetLoanRequestDocumentQueryValidator()
    {
        RuleFor(query => query.LoanRequestId)
            .NotNull().WithMessage("Loan request ID cannot be null.")
            .NotEmpty().WithMessage("Loan request ID is required.");

        RuleFor(query => query.BranchId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Branch ID is required.")
            .GreaterThan(0).WithMessage("Branch ID must be greater than 0.");
    }
}
