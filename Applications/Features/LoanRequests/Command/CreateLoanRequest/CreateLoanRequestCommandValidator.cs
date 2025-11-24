using Applications.Validations.Loans;
using FluentValidation;

namespace Applications.Features.LoanRequests.Command.CreateLoanRequest;

public sealed class CreateLoanRequestCommandValidator : AbstractValidator<CreateLoanRequestCommand>
{
    public CreateLoanRequestCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new CreateLoanRequestValidator());
    }
}
