using Applications.Features.Ledgers.Dto;
using Applications.Interfaces.Transactions;

namespace Applications.Features.Ledgers.Queries.GetLedgerByCurrency;

public sealed class GetLedgerByCurrencyQueryHandler : IRequestHandler<GetLedgerByCurrencyQuery, PageMetaData<VLedger>>
{
    private readonly ILedgerService ledgerService;

    public GetLedgerByCurrencyQueryHandler(ILedgerService ledgerService)
    {
        this.ledgerService = ledgerService;
    }

    public async Task<PageMetaData<VLedger>> Handle(GetLedgerByCurrencyQuery request, CancellationToken cancellationToken)
    {
        return await ledgerService.GetLedgersAsync(request.LedgerId, request.CurrencyId, request.Query, cancellationToken);
    }
}