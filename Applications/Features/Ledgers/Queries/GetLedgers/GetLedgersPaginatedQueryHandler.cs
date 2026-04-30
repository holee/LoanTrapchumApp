using Applications.Interfaces.Transactions;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Ledgers;

namespace Applications.Features.Ledgers.Queries.GetLedgers;

public sealed class GetLedgersPaginatedQueryHandler(ILedgerService ledgerService)
    : IRequestHandler<GetLedgersPaginatedQuery, Pager<VLedger>>
{
    public async Task<Pager<VLedger>> Handle(GetLedgersPaginatedQuery request, CancellationToken cancellationToken)
        => await ledgerService.GetLedgersAsync(request.CurrencyId, request.Param, cancellationToken);
}
