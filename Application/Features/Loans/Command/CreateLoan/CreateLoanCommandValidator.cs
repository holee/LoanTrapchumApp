using FluentValidation;

namespace Application.Features.Loans.Command.CreateLoan;

public sealed class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.Req).SetValidator(new CreateLoanValidator());
    }
}
