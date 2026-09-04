namespace Application.Features.Currencies.Queries.GetSpotRates;

public record GetSpotRatesQuery(int? CurrencyId, QueryParam Query) : IRequest<PageMetaData<CurrencyDtoSpotRate>>; 
