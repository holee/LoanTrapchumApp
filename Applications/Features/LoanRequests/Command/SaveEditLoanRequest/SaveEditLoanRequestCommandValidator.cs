using Applications.Validations.Loans;
using FluentValidation;

namespace Applications.Features.LoanRequests.Command.SaveEditLoanRequest;

public sealed class SaveEditLoanRequestCommandValidator : AbstractValidator<SaveLoanRequestCommand>
{
    public SaveEditLoanRequestCommandValidator()
    { 
        RuleFor(x => x.Request).SetValidator(new SaveLoanRequestValidator());
    }
}
