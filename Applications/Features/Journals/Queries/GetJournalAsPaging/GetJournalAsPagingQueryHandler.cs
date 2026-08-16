using Applications.Interfaces.Transactions;

namespace Applications.Features.Journals.Queries.GetJournalAsPaging;

public sealed class GetJournalAsPagingQueryHandler(IJournalService jService) : IRequestHandler<GetJournalAsPagingQuery, PageMetaData<JournalEntryVm>>
{
    public async Task<PageMetaData<JournalEntryVm>> Handle(GetJournalAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await jService.GetJournalsAsync(request.branchId, request.param, cancellationToken, request.columns);
    }
}