using Application.Validations.Loans;
using FluentValidation;

namespace Application.Features.LoanRequests.Command.SaveLoanRequest;

public sealed class SaveLoanRequestCommandValidator : AbstractValidator<SaveLoanRequestCommand>
{
    public SaveLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new SaveLoanRequestValidator());
    }
}
