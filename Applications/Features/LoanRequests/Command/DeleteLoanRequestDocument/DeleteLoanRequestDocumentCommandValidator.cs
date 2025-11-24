using FluentValidation;

namespace Applications.Features.LoanRequests.Command.DeleteLoanRequestDocument;

public sealed class DeleteLoanRequestDocumentCommandValidator : AbstractValidator<DeleteLoanRequestDocumentCommand>
{
    public DeleteLoanRequestDocumentCommandValidator()
    {
        RuleFor(x => x.LoanRequestId)
            .NotEmpty().WithMessage("Loan Request Id is required.");
        RuleFor(x => x.BranchId)
            .NotNull().WithMessage("Branch Id is required.")
            .GreaterThan(0).WithMessage("Branch Id must be greater than 0.");
        RuleFor(x => x.DocumentId)
            .NotNull().WithMessage("Document Id is required.")
            .GreaterThan(0).WithMessage("Document Id must be greater than 0.");
    }
}