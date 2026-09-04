namespace Application.Features.Currencies.Dto;

/// <summary>
/// Transport shape for the note activate/deactivate call. <see cref="NoteActivateDto"/>
/// only exposes a primary constructor, which System.Text.Json cannot bind reliably.
/// </summary>
public sealed record NoteToggleRequest(bool IsActive);

/// <summary>Wraps a plain text result so HTTP clients always receive a JSON object.</summary>
public sealed record MessageResponse(string Message);
