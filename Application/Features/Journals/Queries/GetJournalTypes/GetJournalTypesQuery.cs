using Application.Features.Journals.Dtos;

namespace Application.Features.Journals.Queries.GetJournalTypes;

public record GetJournalTypesQuery(int? JournalTypeId, QueryParam Param) : IRequest<PageMetaData<JournalTypeDto>>;
