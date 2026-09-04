namespace Application.Features.Journals.Queries.GetJournalAsPaging;

public record GetJournalAsPagingQuery(int? branchId, QueryParam param, params string[] columns) : IRequest<PageMetaData<JournalEntryVm>>;
