using Applications.Features.Journals.Dtos;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalEntryDetailById;

public record GetJournalEntryDetailByIdQuery(string? JournalId) : IRequest<ErrorOr<JournalEntryDto>>;
