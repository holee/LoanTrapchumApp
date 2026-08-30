using Applications.Commons.Mappers;
using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Journals.Command.UpdateJournal;

public sealed class UpdateJournalCommandHandler(IJournalService jService) : IRequestHandler<UpdateJournalCommand, ErrorOr<JournalEntryDto?>>
{
    public async Task<ErrorOr<JournalEntryDto?>> Handle(UpdateJournalCommand request, CancellationToken cancellationToken)
    {

		try
		{
            var model = request.req.MapJournalTransactionUpdate();

            return await jService.CreateOrUpdateAsync(model, cancellationToken);
        }
		catch (Exception ex)
		{

			throw new Exception($"Error in {nameof(UpdateJournalCommandHandler)}: {ex.Message}", ex);
        }

      
    }
}