using Application.Features.Currencies.Responses;
using Application.Interfaces.Transactions;

namespace Application.Features.Currencies.Queries.GetEditMoneyExchange;

public sealed class GetMoneyExchangeQueryAsPagingHandler : IRequestHandler<GetMoneyExchangeAsPagingQuery, Pager<VttCurrencyExchanges>>
{
    private readonly IForiegnCurrencyExchange _fcx;
    public GetMoneyExchangeQueryAsPagingHandler(IForiegnCurrencyExchange repository)
    {
        _fcx = repository;
    }
    public async Task<Pager<VttCurrencyExchanges>> Handle(GetMoneyExchangeAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await _fcx.GetCurrenciesExchange(request.branchId, request.TransactionDate, request.Param, cancellationToken);

    }
}