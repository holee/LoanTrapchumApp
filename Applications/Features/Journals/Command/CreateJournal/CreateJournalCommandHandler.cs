using Applications.Commons.Mappers;
using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Journals.Command.CreateJournal;

public sealed class CreateJournalCommandHandler(IJournalService jService) : IRequestHandler<CreateJournalCommand, ErrorOr<JournalEntry?>>
{
    public async Task<ErrorOr<JournalEntry?>> Handle(CreateJournalCommand request, CancellationToken cancellationToken)
    {
        var model = request.req.MapTransactionCreate();

        return await jService.CreateAsync(model, cancellationToken);
    }
}