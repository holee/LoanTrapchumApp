using FluentValidation;

namespace Applications.Features.Customers.Commands.NotesCommand.DeleteNoteCommand;

public sealed class DeleteNoteCommandHandlerValidator : AbstractValidator<DeleteNoteCommand>
{

    public DeleteNoteCommandHandlerValidator()
    {
        RuleFor(x => x.NoteId).GreaterThan(0)
                                .WithMessage("Note ID is required.");

        RuleFor(x=>x.CustomerId).NotEmpty()
                    .WithMessage("Customer ID is required.");
    }

}
