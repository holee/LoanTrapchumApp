using FluentValidation;

namespace Applications.Features.LoanRequests.Command.DenyLoanRequest;

public sealed class DeniedLoanRequestCommandValidator : AbstractValidator<DeniedLoanRequestCommand>
{
    public DeniedLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new DeniedLoanRequestValidator());
    }
}
