using FluentValidation;

namespace Applications.Features.LoanRequests.Command.ApprovedLoanRequest
{
    public sealed class ApprovedLoanRequestCommandValidator : AbstractValidator<ApprovedRequestCommand>
    {
        public ApprovedLoanRequestCommandValidator()
        {
            RuleFor(x => x.Request).SetValidator(new AprovedLoanDtoCreateValidator());
        }
    }
}
