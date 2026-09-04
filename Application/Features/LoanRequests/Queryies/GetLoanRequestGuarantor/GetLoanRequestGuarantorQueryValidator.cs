using FluentValidation;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestGuarantor;

public sealed class GetLoanRequestGuarantorQueryValidator : AbstractValidator<GetLoanRequestGuarantorQuery>
{
    public GetLoanRequestGuarantorQueryValidator()
    {
        RuleFor(x => x.LoanRequestId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Loan Request Id is required.")
            .NotNull().WithMessage("Loan Request Id must not exceed 50 characters.");
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Customer Id is required.")
            .NotNull().WithMessage("Customer Id must not exceed 50 characters.");
    }
}
