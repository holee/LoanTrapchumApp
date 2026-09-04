using Application.Validations.Loans;
using FluentValidation;

namespace Application.Features.LoanRequests.Command.CreateLoanRequest;

public sealed class CreateLoanRequestCommandValidator : AbstractValidator<CreateLoanRequestCommand>
{
    public CreateLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new CreateLoanRequestValidator());
    }
}
