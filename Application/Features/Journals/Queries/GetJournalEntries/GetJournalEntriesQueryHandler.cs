using Application.Commons;
using Application.Features.Journals.Dtos;
using Application.Interfaces.Transactions;

namespace Application.Features.Journals.Queries.GetJournalEntries;

public sealed class GetJournalEntriesQueryHandler : IRequestHandler<GetJournalEntriesQuery, PageMetaData<JournalEntryDto>>
{
    private readonly IJournalService _journalService;
    public GetJournalEntriesQueryHandler(IJournalService journalService)
    {
        _journalService = journalService;
    }
    public async Task<PageMetaData<JournalEntryDto>> Handle(GetJournalEntriesQuery request, CancellationToken cancellationToken)
    {
        return await _journalService.GetJournalsAsync(request.JournalEntryId, request.Param, cancellationToken);
    }
}