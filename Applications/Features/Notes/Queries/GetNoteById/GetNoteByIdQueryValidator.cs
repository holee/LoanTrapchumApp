using FluentValidation;

namespace Applications.Features.Notes.Queries.GetNoteById;

public sealed class GetNoteByIdQueryValidator : AbstractValidator<GetNoteByIdQuery>
{
    public GetNoteByIdQueryValidator()
    {
        RuleFor(x => x.NoteId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("NoteId is required.")
            .GreaterThan(0).WithMessage("NoteId must be greater than 0.");
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("CustomerId is required.")
            .NotEmpty().WithMessage("CustomerId cannot be empty.");
    }
}
