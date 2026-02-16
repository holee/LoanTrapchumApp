using Applications.Interfaces.Currencies;
using Domains.Models.Currencies;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetCurrencyById;

public sealed class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, ErrorOr<CurrencyDto>>
{
    private readonly ICurrencyService _currencyRepository;
    public GetCurrencyByIdQueryHandler(ICurrencyService currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }
    public async Task<ErrorOr<CurrencyDto>> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _currencyRepository.GetCurrencyById(request.CurrencyId, cancellationToken, request.includes);
    }
}
