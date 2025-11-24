using Applications.Features.LoanRequests.Requests;
using FluentValidation;

namespace Applications.Features.LoanRequests.Command.DenyLoanRequest;

public sealed class DeniedLoanRequestValidator : AbstractValidator<DeniedRequestDtoCreate>
{
    public DeniedLoanRequestValidator()
    {
        RuleFor(x => x.LoanRequestId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please select request to denied.")
            .NotNull().WithMessage("Please select request to denied.");
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please select customer.")
            .NotNull().WithMessage("Please select customer.");
    }
}
