using FluentValidation;

namespace Applications.Features.Customers.Commands.NotesCommand.EditNoteCommand;

public sealed class EditDtoUpdateCommandValidator:AbstractValidator<EditNoteCommand>
{
    public EditDtoUpdateCommandValidator()
    {
        RuleFor(x=>x.NoteId)
            .GreaterThan(0).WithMessage("NoteId must be greater than zero");
        RuleFor(x=>x.CustomerRefId)
            .NotEmpty().WithMessage("CustomerRefId must not be empty");
        RuleFor(x=>x.CreditAgentRefId)
            .NotEmpty().WithMessage("CreditAgentRefId must not be empty");
        RuleFor(x=>x.Description)
            .NotEmpty().WithMessage("Description must not be empty");
    }
}