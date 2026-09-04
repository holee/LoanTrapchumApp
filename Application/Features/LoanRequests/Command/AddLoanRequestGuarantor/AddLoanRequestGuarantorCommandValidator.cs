using FluentValidation;

namespace Application.Features.LoanRequests.Command.AddLoanRequestGuarantor;

public sealed class AddLoanRequestGuarantorCommandValidator : AbstractValidator<AddLoanRequestGuarantorCommand>
{
    public AddLoanRequestGuarantorCommandValidator()
    {
        RuleFor(x => x.Request).SetValidator(new AddLoanRequestGuarantorValidator());
    }
}
