using Applications.Features.Journals.Dtos;

namespace Applications.Features.Journals.Queries.GetJournalTypes;

public record GetJournalTypesQuery(int? JournalTypeId, QueryParam Param) : IRequest<PageMetaData<JournalTypeDto>>;
