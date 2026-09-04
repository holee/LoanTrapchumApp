using FluentValidation;

namespace Application.Features.LoanRequests.Command.AddLoanRequestDescription;

public sealed class AddLoanRequestDescriptionCommandValidator
    : AbstractValidator<AddLoanRequestDescriptionCommand>
{
    public AddLoanRequestDescriptionCommandValidator()
    {
        RuleFor(x => x.Req).SetValidator(new LoanDescriptionsDtoCreateValidator());
    }
}
