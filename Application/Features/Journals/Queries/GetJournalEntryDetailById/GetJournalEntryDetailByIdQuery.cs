using Application.Features.Journals.Dtos;
using ErrorOr;

namespace Application.Features.Journals.Queries.GetJournalEntryDetailById;

public record GetJournalEntryDetailByIdQuery(string? JournalId, bool Include = false) : IRequest<ErrorOr<JournalEntryDto>>;
