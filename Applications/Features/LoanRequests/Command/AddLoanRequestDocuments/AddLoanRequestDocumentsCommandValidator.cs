using FluentValidation;

namespace Applications.Features.LoanRequests.Command.AddLoanRequestDocument;

public sealed class AddLoanRequestDocumentsCommandValidator : AbstractValidator<AddLoanRequestDocumentsCommand>
{
    public AddLoanRequestDocumentsCommandValidator()
    {
        RuleFor(x => x.Requests).ForEach(x => x.SetValidator(new AddLoanRequestDocumentValidator()));
    }
}
