using Application.Features.Journals.Dtos;
using Application.Interfaces.Transactions;

namespace Application.Features.Journals.Queries.GetJournalTypes;

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