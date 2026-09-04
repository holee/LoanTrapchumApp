using Application.Features.Ledgers.Dto;
using Application.Interfaces.Transactions;

namespace Application.Features.Ledgers.Queries.GetLedgerByCurrency;

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