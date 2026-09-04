using Application.Features.LoanRequests.Requests;
using FluentValidation;

namespace Application.Features.LoanRequests.Command.AddLoanRequestGuarantor;

public sealed class AddLoanRequestGuarantorValidator : AbstractValidator<LoanRequestGuarantorsDtoCreate>
{
    public AddLoanRequestGuarantorValidator()
    {
        RuleFor(x => x.LoanRequestId)
            .NotEmpty().WithMessage("Loan Request ID is required.");
        RuleFor(x => x.CustomerRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Customer Reference ID is required.")
            .NotEmpty().WithMessage("Customer Reference ID is required.");
        RuleFor(x => x.BranchRefId)
            .NotNull().WithMessage("Branch Reference ID is required.")
            .GreaterThan(0).WithMessage("Branch Reference ID must be greater than zero.");
      
    }
}