using Applications.Interfaces.Transactions;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalById;

public sealed class GetJournalByIdQueryHandler : IRequestHandler<GetJournalByIdQuery, ErrorOr<JournalEntry>>
{
    private readonly IJournalService _journalService;
    public GetJournalByIdQueryHandler(IJournalService journalService)
    {
        _journalService = journalService;
    }
    public async Task<ErrorOr<JournalEntry>> Handle(GetJournalByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _journalService.GetJournalByIdAsync(request.JournalId, cancellationToken);

        if (result == null)
        {
            return Error.NotFound("JournalEntry.NotFound", $"Journal entry with ID {request.JournalId} was not found.");
        }

        return result;
    }
}