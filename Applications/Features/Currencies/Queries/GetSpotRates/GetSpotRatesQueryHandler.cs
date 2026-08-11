using Applications.Interfaces.Transactions;

namespace Applications.Features.Currencies.Queries.GetSpotRates;

public sealed class GetSpotRatesQueryHandler : IRequestHandler<GetSpotRatesQuery, PageMetaData<CurrencyDtoSpotRate>>
{
    private readonly ISpotRateService _spotRateService;
    public GetSpotRatesQueryHandler(ISpotRateService spotRateService)
    {
        _spotRateService = spotRateService;
    }
    public async Task<PageMetaData<CurrencyDtoSpotRate>> Handle(GetSpotRatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _spotRateService.GetSpotRates(request.CurrencyId, request.Query);

        return result;
    }
}