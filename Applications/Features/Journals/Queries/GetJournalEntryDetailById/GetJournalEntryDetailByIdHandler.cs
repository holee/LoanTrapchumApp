using Applications.Features.Journals.Dtos;
using Applications.Features.Journals.Queries.GetJournalById;
using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalEntryDetailById;

public sealed class GetJournalEntryDetailByIdHandler : IRequestHandler<GetJournalEntryDetailByIdQuery, ErrorOr<JournalEntryDto>>
{
    private readonly IJournalService _journalService;
    public GetJournalEntryDetailByIdHandler(IJournalService journalService)
    {
        _journalService = journalService;
    }
    public async Task<ErrorOr<JournalEntryDto>> Handle(GetJournalEntryDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _journalService.GetJournalDetailByIdAsync(request.JournalId, cancellationToken);

        if (result == null)
        {
            return Error.NotFound("JournalEntry.NotFound", $"Journal entry with ID {request.JournalId} was not found.");
        }

        return result;
    }
}