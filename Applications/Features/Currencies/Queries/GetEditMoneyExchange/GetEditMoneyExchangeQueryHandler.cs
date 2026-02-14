using Applications.Commons.Mappers.Exchanges;
using Applications.Exceptions;
using Applications.Features.Currencies.Dto;
using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetEditMoneyExchange;

public sealed class GetEditMoneyExchangeQueryHandler : IRequestHandler<GetEditMoneyExchangeQuery, ErrorOr<ExchangeDtoEdit>>
{
    private readonly IForiegnCurrencyExchange _fcx;
    public GetEditMoneyExchangeQueryHandler(IForiegnCurrencyExchange repository)
    {
        _fcx = repository;
    }
    public async Task<ErrorOr<ExchangeDtoEdit>> Handle(GetEditMoneyExchangeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _fcx.GetCurrencyExchange(request.branchId, request.TransactionId, request.TransactionDate, cancellationToken);

        if (entity is null)
        {
            return Error.NotFound("NotFound", "Currency Exchange Transaction not found.");
        }

        return entity.ToDtoEdit();
    }
}