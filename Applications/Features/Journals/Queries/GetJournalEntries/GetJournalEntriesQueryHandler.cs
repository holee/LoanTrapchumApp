using Applications.Commons;
using Applications.Features.Journals.Dtos;
using Applications.Interfaces.Transactions;

namespace Applications.Features.Journals.Queries.GetJournalEntries;

public sealed class GetJournalEntriesQueryHandler : IRequestHandler<GetJournalEntriesQuery, PaginationResult<JournalEntryDto>>
{
    private readonly IJournalService _journalService;
    public GetJournalEntriesQueryHandler(IJournalService journalService)
    {
        _journalService = journalService;
    }
    public async Task<PaginationResult<JournalEntryDto>> Handle(GetJournalEntriesQuery request, CancellationToken cancellationToken)
    {
        return await _journalService.GetJournalsAsync(request.JournalEntryId, request.Param, cancellationToken);
    }
}