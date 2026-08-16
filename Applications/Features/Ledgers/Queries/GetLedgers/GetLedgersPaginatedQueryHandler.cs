using Applications.Features.Ledgers.Dto;
using Applications.Interfaces.Transactions;

namespace Applications.Features.Ledgers.Queries.GetLedgers;

public sealed class GetLedgersPaginatedQueryHandler(ILedgerService ledgerService)
    : IRequestHandler<GetLedgersPaginatedQuery, PageMetaData<VLedger>>
{
    public async Task<PageMetaData<VLedger>> Handle(GetLedgersPaginatedQuery request, CancellationToken cancellationToken)
        => await ledgerService.GetLedgersAsync(request.CurrencyId, request.Param, cancellationToken, request.Columns);
}
