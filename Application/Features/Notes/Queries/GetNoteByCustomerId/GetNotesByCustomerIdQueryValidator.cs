using FluentValidation;

namespace Application.Features.Notes.Queries.GetNoteByCustomerId;

public sealed class GetNotesByCustomerIdQueryValidator : AbstractValidator<GetNotesByCustomerIdQuery>
{
    public GetNotesByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
    }
}
