using Applications.Validations.Loans;
using FluentValidation;

namespace Applications.Features.LoanRequests.Command.SaveLoanRequest;

public sealed class SaveLoanRequestCommandValidator : AbstractValidator<SaveLoanRequestCommand>
{
    public SaveLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new SaveLoanRequestValidator());
    }
}
