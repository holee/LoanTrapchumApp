using Application.Features.Currencies.Responses;
using Application.Interfaces.Currencies;

namespace Application.Features.Currencies.Queries.GetCurrencyCounter;

public sealed class GetCurrencyCounterQueryHandler(ICurrencyService currencyService) : IRequestHandler<GetCurrencyCounterQuery, Pager<VCurrencyCounter>>
{
    public async Task<Pager<VCurrencyCounter>> Handle(GetCurrencyCounterQuery request, CancellationToken cancellationToken)
    {
        return await currencyService.GetCurrencyCounter(request.BranchId, request.CurrencyId, cancellationToken);
    }
}