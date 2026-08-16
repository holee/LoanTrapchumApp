using Applications.Features.Journals.Dtos;
using Applications.Interfaces.Transactions;

namespace Applications.Features.Journals.Queries.GetJournalTypes;

public sealed class GetJournalTypesQueryHandler : IRequestHandler<GetJournalTypesQuery, PageMetaData<JournalTypeDto>>
{

    private readonly IJournalService _journalService;

    public GetJournalTypesQueryHandler(IJournalService journalService)
    {
        _journalService = journalService;
    }

    public async Task<PageMetaData<JournalTypeDto>> Handle(GetJournalTypesQuery request, CancellationToken cancellationToken)
    {
       var result = await _journalService.GetJournalTypesAsync(request.JournalTypeId, request.Param, cancellationToken);
       return result;
    }
}