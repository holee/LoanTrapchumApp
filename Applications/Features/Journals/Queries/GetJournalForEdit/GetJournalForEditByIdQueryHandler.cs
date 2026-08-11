using Applications.Commons.Mappers;
using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalForEdit;

public sealed class GetJournalForEditByIdQueryHandler : IRequestHandler<GetJournalForEditByIdQuery, ErrorOr<JournalEntryDtoUpdate>>
{
    private readonly IJournalService _journalService;
    public GetJournalForEditByIdQueryHandler(IJournalService journalService)
    {
        _journalService = journalService;
    }
    public async Task<ErrorOr<JournalEntryDtoUpdate>> Handle(GetJournalForEditByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _journalService.GetJournalById(request.TranxId, cancellationToken);

            if (result == null)
            {
                return Error.NotFound("JournalEntry.NotFound", $"Journal entry with ID {request.TranxId} was not found.");
            }

            return result.MapToJournalEntryDtoUpdate();
        }
        catch (Exception)
        {

            throw new Exception($"An error occurred while retrieving the journal entry with ID {request.TranxId}.");
        }
       

    }
}