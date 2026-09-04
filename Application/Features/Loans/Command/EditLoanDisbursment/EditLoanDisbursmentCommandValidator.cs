using FluentValidation;

namespace Application.Features.Loans.Command.EditLoanDisbursment;

public sealed class EditLoanDisbursmentCommandValidator : AbstractValidator<EditLoanDisbursmentCommand>
{
    public EditLoanDisbursmentCommandValidator()
    {
        RuleFor(x => x.Req).SetValidator(new EditLoanDisbursmentValidator());
    }
}
