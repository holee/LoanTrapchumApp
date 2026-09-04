using Application.Interfaces.Transactions;
using ErrorOr;

namespace Application.Features.Ledgers.Commands.DeleteLedger;

public sealed class DeleteLedgerCommandHandler(ILedgerService ledgerService)
    : IRequestHandler<DeleteLedgerCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteLedgerCommand request, CancellationToken cancellationToken)
        => await ledgerService.DeleteLedger(request.Id);
}
