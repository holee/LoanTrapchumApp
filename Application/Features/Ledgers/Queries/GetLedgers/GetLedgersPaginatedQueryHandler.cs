using Application.Features.Ledgers.Dto;
using Application.Interfaces.Transactions;

namespace Application.Features.Ledgers.Queries.GetLedgers;

public sealed class GetLedgersPaginatedQueryHandler(ILedgerService ledgerService)
    : IRequestHandler<GetLedgersPaginatedQuery, PageMetaData<VLedger>>
{
    public async Task<PageMetaData<VLedger>> Handle(GetLedgersPaginatedQuery request, CancellationToken cancellationToken)
        => await ledgerService.GetLedgersAsync(request.CurrencyId, request.Param, cancellationToken, request.Columns);
}
