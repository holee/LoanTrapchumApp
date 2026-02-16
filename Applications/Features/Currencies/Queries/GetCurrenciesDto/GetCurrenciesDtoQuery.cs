namespace Applications.Features.Currencies.Queries.GetCurrencies;

public record GetCurrenciesDtoQuery(int? currencyId, QueryParam Param) : IRequest<Pager<CurrencyDto>>;
