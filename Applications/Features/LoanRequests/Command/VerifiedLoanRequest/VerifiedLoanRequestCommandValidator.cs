using FluentValidation;

namespace Applications.Features.LoanRequests.Command.VerifiedLoanRequest;

public sealed class VerifiedLoanRequestCommandValidator : AbstractValidator<VerifiedLoanRequestCommand>
{
    public VerifiedLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new VerifiedLoanRequestValidator());
    }
}
