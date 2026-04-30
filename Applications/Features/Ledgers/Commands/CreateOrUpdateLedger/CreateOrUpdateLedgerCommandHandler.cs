using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Ledgers.Commands.CreateOrUpdateLedger;

public sealed class CreateOrUpdateLedgerCommandHandler(ILedgerService ledgerService)
    : IRequestHandler<CreateOrUpdateLedgerCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(CreateOrUpdateLedgerCommand request, CancellationToken cancellationToken)
        => await ledgerService.CreateOrUpdateLedger(request.Entity);
}
