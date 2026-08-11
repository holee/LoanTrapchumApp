using Applications.Interfaces.Transactions;

namespace Applications.Features.Currencies.Queries.GetCurrenciesForSetSpotRates;

public sealed class GetCurrenciesForSetSpotRatesQueryHandler : IRequestHandler<GetCurrenciesForSetSpotRatesQuery, PageMetaData<VCurrenciesList>>
{
    private readonly ISpotRateService _spotRateService;
    public GetCurrenciesForSetSpotRatesQueryHandler(ISpotRateService spotRateService)
    {
        _spotRateService = spotRateService;
    }
    public async Task<PageMetaData<VCurrenciesList>> Handle(GetCurrenciesForSetSpotRatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _spotRateService.GetCurrenciesForSetSpotRate(request.IsBaseCurrency, request.Query);

        return result;
    }
}