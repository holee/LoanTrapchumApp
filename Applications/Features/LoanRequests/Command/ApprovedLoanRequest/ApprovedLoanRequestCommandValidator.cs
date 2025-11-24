using FluentValidation;

namespace Applications.Features.LoanRequests.Command.ApprovedLoanRequest
{
    public sealed class ApprovedLoanRequestCommandValidator : AbstractValidator<ApprovedLoanRequestCommand>
    {
        public ApprovedLoanRequestCommandValidator()
        {
            RuleFor(x => x.Request).SetValidator(new AprovedLoanDtoCreateValidator());
        }
    }
}
