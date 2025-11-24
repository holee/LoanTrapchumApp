using Applications.Features.LoanRequests.Requests;
using FluentValidation;

namespace Applications.Features.LoanRequests.Command.AddLoanRequestDescription;

public sealed class LoanDescriptionsDtoCreateValidator : AbstractValidator<LoanDescriptionsDtoCreate>
{
    public LoanDescriptionsDtoCreateValidator()
    {
        RuleFor(x => x.LoanRequestRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Loan Request Ref Id is required.")
            .NotEmpty().WithMessage("Loan Request Id is required.");
        RuleFor(x => x.UserRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("User Ref Id is required.")
            .NotEmpty().WithMessage("User Ref Id is required.");
    }
}