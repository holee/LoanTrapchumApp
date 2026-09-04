using Application.Features.Loans.Command.SaveLoanDisbursemenent;

namespace Application.Features.LoanRequests.Command.SaveLoanRequests;

public class SaveLoanRequestsCommandValidator : AbstractValidator<SaveLoanRequestByManagerCommand>
{
    public SaveLoanRequestsCommandValidator()
    {
        RuleFor(x => x.Request).NotNull().WithMessage("Request cannot be null.");
        RuleFor(x => x.Request).SetValidator(new SaveLoanDisbursementDtoCreateValidator());
    }
}
