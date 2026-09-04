using Application.Validations.Loans;
using FluentValidation;

namespace Application.Features.LoanRequests.Command.SaveEditLoanRequest;

public sealed class SaveEditLoanRequestCommandValidator : AbstractValidator<SaveLoanRequestCommand>
{
    public SaveEditLoanRequestCommandValidator()
    { 
        RuleFor(x => x.Request).SetValidator(new SaveLoanRequestValidator());
    }
}
