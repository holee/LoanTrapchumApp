using Applications.Interfaces.Transactions;
using Domains.Models.Journals;
using Domains.Models.Transaction;
using ErrorOr;

namespace Applications.Features.Journals.Queries.GetJournalTransaction;

public sealed class GetJournalTransactionByIdQueryHandler : IRequestHandler<GetJournalTransactionByIdQuery, ErrorOr<LoanTransaction>>
{
    private readonly IJournalService _journalService;
    public GetJournalTransactionByIdQueryHandler(IJournalService journalService)
    {
        _journalService = journalService;
    }
    public async Task<ErrorOr<LoanTransaction>> Handle(GetJournalTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {

            var result = await _journalService.GetJournalById(request.TransactionId, request.BranchId, cancellationToken);

            if (result == null)
            {
                return Error.NotFound("JournalEntry.NotFound", $"Journal entry with ID {request.TransactionId} was not found.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the journal entry.", ex);
        }

    }
}