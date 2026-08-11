using Applications.Interfaces.Transactions;
using Applications.Responses.ViewModels.Ledgers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Ledgers.Queries.GetLedgerByCurrency;

public record GetLedgerByCurrencyQuery(int? LedgerId, int? CurrencyId, QueryParam Query)
              : IRequest<PageMetaData<VLedger>>;



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