using Application.Commons.Mappers;
using Application.Interfaces.Transactions;
using ErrorOr;

namespace Application.Features.Journals.Queries.GetJournalEntryForUpdate;

public sealed class GetJournalEntryForUpdateIdQueryHandler : IRequestHandler<GetJournalEntryForUpdateByIdQuery, ErrorOr<JournalTransactionDtoUpdate>>
{
    private readonly IJournalService _journalService;
    public GetJournalEntryForUpdateIdQueryHandler(IJournalService journalService)
    {
        _journalService = journalService;
    }
    public async Task<ErrorOr<JournalTransactionDtoUpdate>> Handle(GetJournalEntryForUpdateByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {

            var result = await _journalService.GetJournalById(request.TransactionId, request.BranchId, cancellationToken);

            if (result == null)
            {
                return Error.NotFound("JournalEntry.NotFound", $"Journal entry with ID {request.TransactionId} was not found.");
            }
            return result.MapTransactionUpdate();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the journal entry {ex.Message}.", ex);
        }

    }
}