using Applications.Commons;
using Applications.Features.Journals.Dtos;

namespace Applications.Features.Journals.Queries.GetJournalEntries;

public record GetJournalEntriesQuery(string? JournalEntryId, QueryParam Param) : IRequest<PageMetaData<JournalEntryDto>>;
