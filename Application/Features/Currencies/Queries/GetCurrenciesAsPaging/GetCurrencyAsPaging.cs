using Application.Interfaces.Currencies;

namespace Application.Features.Currencies.Queries.GetCurrenciesAsPaging;

public record GetCurrencyAsPaging(QueryParam Param) : IRequest<Pager<VCurrenciesList>>;

public class GetCurrencyAsPagingHandler(ICurrencyService service) : IRequestHandler<GetCurrencyAsPaging, Pager<VCurrenciesList>>
{
    public async Task<Pager<VCurrenciesList>> Handle(GetCurrencyAsPaging request, CancellationToken cancellationToken)
    {
        return await service.GetCurrencies(request.Param);
    }
}