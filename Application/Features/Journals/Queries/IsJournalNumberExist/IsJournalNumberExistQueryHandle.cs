using Application.Interfaces.Transactions;
using ErrorOr;

namespace Application.Features.Journals.Queries.IsJournalNumberExist;

public sealed class IsJournalNumberExistQueryHandle : IRequestHandler<IsJournalNumberExistQuery, ErrorOr<bool>>
{
    private readonly IJournalService _journanlService;

    public IsJournalNumberExistQueryHandle(IJournalService journanlService) => _journanlService = journanlService;

    public async Task<ErrorOr<bool>> Handle(IsJournalNumberExistQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _journanlService.IsNumberIsAllreadyUsed(request.JournalNumber, request.BranchId, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while checking if journal number exists: {ex.Message}", ex);
        }

    }
}