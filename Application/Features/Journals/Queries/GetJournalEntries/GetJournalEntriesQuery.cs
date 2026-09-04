using Application.Commons;
using Application.Features.Journals.Dtos;

namespace Application.Features.Journals.Queries.GetJournalEntries;

public record GetJournalEntriesQuery(string? JournalEntryId, QueryParam Param) : IRequest<PageMetaData<JournalEntryDto>>;
