using FluentValidation;

namespace Applications.Features.Customers.Commands.NotesCommand.CreateNoteCommand;

public sealed class CreateNoteCommandHandlerValidator :AbstractValidator<AddNoteCommand>
{
    public CreateNoteCommandHandlerValidator()
    {
        RuleFor(x=>x.CustomerRefId)
            .NotEmpty()
            .WithMessage("CustomerRefId is required");
        RuleFor(x=>x.Description)
            .NotEmpty()
            .WithMessage("Description is required");
    }
}