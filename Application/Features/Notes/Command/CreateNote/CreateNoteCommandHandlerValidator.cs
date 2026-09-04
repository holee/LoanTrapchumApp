using FluentValidation;

namespace Application.Features.Notes.Command.CreateNote;

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