using Applications.Interfaces.Transactions;
using Domains.Models.Journals;
using ErrorOr;

namespace Applications.Features.Ledgers.Queries.GetLedgerById;

public sealed class GetLedgerByIdQueryHandler(ILedgerService ledgerService)
    : IRequestHandler<GetLedgerByIdQuery, ErrorOr<Ledger>>
{
    public async Task<ErrorOr<Ledger>> Handle(GetLedgerByIdQuery request, CancellationToken cancellationToken)
        => await ledgerService.GetLedgerByIdAsync(request.LedgerId);
}
