namespace Application.Features.LoanRequests.Command.RecheckedLoanRequest;

public sealed class RecheckedLoanRequestCommandValidator : AbstractValidator<RecheckedLoanRequestCommand>
{
    public RecheckedLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new RecheckedRequestValidator());
    }
}