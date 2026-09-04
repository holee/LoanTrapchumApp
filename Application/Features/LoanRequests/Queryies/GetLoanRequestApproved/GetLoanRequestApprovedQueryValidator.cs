using FluentValidation;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestBy;

public sealed class GetLoanRequestApprovedQueryValidator : AbstractValidator<GetLoanRequestApprovedQuery>
{
    public GetLoanRequestApprovedQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Branch Id is required.")
            .GreaterThan(0).WithMessage("Branch Id must be greater than zero.");
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Customer Id cannot be null.")
            .NotEmpty().WithMessage("Customer Id is required.");
        RuleFor(x => x.LoanRequestId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Loan Request Id cannot be null.")
            .NotEmpty().WithMessage("Loan Request Id is required.");
    }
}
