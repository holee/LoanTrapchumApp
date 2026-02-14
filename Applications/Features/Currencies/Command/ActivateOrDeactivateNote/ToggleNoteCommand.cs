using Applications.Features.Currencies.Dto;
using Applications.Interfaces.Currencies;
using ErrorOr;

namespace Applications.Features.Currencies.Command.ActivateOrDeactivateNote;

public record ToggleNoteCommand(int? noteId, NoteActivateDto Note) : IRequest<ErrorOr<string>>;


public sealed class ToggleNoteCommandValidator : AbstractValidator<ToggleNoteCommand>
{
    public ToggleNoteCommandValidator()
    {
        RuleFor(x => x.noteId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Note ID cannot be null.")
                .GreaterThan(0).WithMessage("Note ID must be greater than 0.");
    }
}

public class ToggleNoteCommandHandler : IRequestHandler<ToggleNoteCommand, ErrorOr<string>>
{
    private readonly ICurrencyService _currencyService;
    public ToggleNoteCommandHandler(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }
    public async Task<ErrorOr<string>> Handle(ToggleNoteCommand request, CancellationToken cancellationToken)
    {
        return await _currencyService.ActivateOrDeactivateNote(request.noteId, request.Note, cancellationToken);
    }
}