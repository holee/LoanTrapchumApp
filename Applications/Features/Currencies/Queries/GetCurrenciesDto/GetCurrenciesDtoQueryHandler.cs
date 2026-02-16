using Applications.Interfaces.Currencies;

namespace Applications.Features.Currencies.Queries.GetCurrencies;

public sealed class GetCurrenciesDtoQueryHandler(ICurrencyService currencyRepository) : IRequestHandler<GetCurrenciesDtoQuery, Pager<CurrencyDto>>
{
    public async Task<Pager<CurrencyDto>> Handle(GetCurrenciesDtoQuery request, CancellationToken cancellationToken)
    {
        return await currencyRepository.GetCurrenciesAsync(request.currencyId, request.Param);
    }
}
