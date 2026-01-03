using Applications.Commons.Mappers.Exchanges;
using Applications.Exceptions;
using Applications.Features.Currencies.Requests;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetEditMoneyExchange;

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