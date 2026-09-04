namespace Application.Features.Currencies.Queries.GetCurrenciesForSetSpotRates;

public record GetCurrenciesForSetSpotRatesQuery(bool IsBaseCurrency, QueryParam Query) : IRequest<PageMetaData<VCurrenciesList>>;
