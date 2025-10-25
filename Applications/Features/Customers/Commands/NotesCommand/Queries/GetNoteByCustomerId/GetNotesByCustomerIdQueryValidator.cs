using FluentValidation;

namespace Applications.Features.Customers.Commands.NotesCommand.Queries.GetNoteByCustomerId;

public sealed class GetNotesByCustomerIdQueryValidator : AbstractValidator<GetNotesByCustomerIdQuery>
{
    public GetNotesByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
    }
}
