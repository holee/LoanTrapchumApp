using Applications.Features.LoanRequests.Requests;
using FluentValidation;

namespace Applications.Features.LoanRequests.Command.AddLoanRequestDocument;

public sealed class AddLoanRequestDocumentValidator : AbstractValidator<LoanDocumentDtoCreate>
{
    public AddLoanRequestDocumentValidator() 
    {
        RuleFor(x => x.LoanRequestId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("LoanRequestId is required.")
            .NotEmpty().WithMessage("LoanRequestId is required.");
        RuleFor(x => x.BranchId)
            .NotNull().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than zero.");

    }
}