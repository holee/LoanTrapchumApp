using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Ledgers.Commands.ChangeLedgerStatus;

public sealed class ChangeLedgerStatusCommandHandler(ILedgerService ledgerService)
    : IRequestHandler<ChangeLedgerStatusCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(ChangeLedgerStatusCommand request, CancellationToken cancellationToken)
        => await ledgerService.ChangeLedgerStatus(request.Req);
}
