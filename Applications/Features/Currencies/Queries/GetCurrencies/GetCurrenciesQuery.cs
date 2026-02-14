using Applications.Interfaces.Currencies;
using Domains.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Currencies.Queries.GetCurrencies;

public record GetCurrenciesQuery(int? currencyId, QueryParam Param) : IRequest<Pager<Currency>>;

public sealed class GetCurrenciesQueryHandler(ICurrencyService currencyRepository) : IRequestHandler<GetCurrenciesQuery, Pager<Currency>>
{
    public async Task<Pager<Currency>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return await currencyRepository.GetCurrencies(request.currencyId, request.Param);
    }
}
