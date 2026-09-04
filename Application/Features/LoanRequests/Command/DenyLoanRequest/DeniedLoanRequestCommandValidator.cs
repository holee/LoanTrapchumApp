using FluentValidation;

namespace Application.Features.LoanRequests.Command.DenyLoanRequest;

public sealed class DeniedLoanRequestCommandValidator : AbstractValidator<DeniedLoanRequestCommand>
{
    public DeniedLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new DeniedLoanRequestValidator());
    }
}
