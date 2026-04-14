using Applications.Features.Loans.Command.SaveLoanDisbursemenent;

namespace Applications.Features.LoanRequests.Command.SaveLoanRequests;

public class SaveLoanRequestsCommandValidator : AbstractValidator<SaveLoanRequestByManagerCommand>
{
    public SaveLoanRequestsCommandValidator()
    {
        RuleFor(x => x.Request).NotNull().WithMessage("Request cannot be null.");
        RuleFor(x => x.Request).SetValidator(new SaveLoanDisbursementDtoCreateValidator());
    }
}
