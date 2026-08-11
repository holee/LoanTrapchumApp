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
        try
        {
            var result = await _journalService.GetJournalByIdAsync(request.JournalId, request.Include, cancellationToken);

            if (result == null)
            {
                return Error.NotFound("JournalEntry.NotFound", $"Journal entry with ID {request.JournalId} was not found.");
            }

            return result;
        }
        catch (Exception ex)
        {

            throw new Exception($" {ex.Message}");
        }

    }
}