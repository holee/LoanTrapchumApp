using Applications.Interfaces.Currencies;

namespace Applications.Features.Currencies.Queries.GetCurrencies;

public sealed class GetCurrenciesQueryHandler(ICurrencyService currencyRepository) : IRequestHandler<GetCurrenciesQuery, PageMetaData<CurrencyDto>>
{
    public async Task<PageMetaData<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await currencyRepository.GetCurrenciesAsync(request.currencyId, request.Param, cancellationToken, request.Columns);
        }
        catch (Exception ex)
        {
            throw new Exception("", ex);
        }
    }
}
