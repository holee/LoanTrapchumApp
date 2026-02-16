using Applications.Interfaces.Currencies;

namespace Applications.Features.Currencies.Queries.GetCurrencies;

public sealed class GetCurrenciesQueryHandler(ICurrencyService currencyRepository) : IRequestHandler<GetCurrenciesQuery, Pager<Currency>>
{
    public async Task<Pager<Currency>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return await currencyRepository.GetCurrencies(request.currencyId, request.Param);
    }
}
