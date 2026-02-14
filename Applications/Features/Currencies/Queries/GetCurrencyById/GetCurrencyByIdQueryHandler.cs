using Applications.Interfaces.Currencies;
using Domains.Models.Currencies;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetCurrencyDetails;

public sealed class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, ErrorOr<Currency>>
{
    private readonly ICurrencyService _currencyRepository;
    public GetCurrencyByIdQueryHandler(ICurrencyService currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }
    public async Task<ErrorOr<Currency>> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _currencyRepository.GetCurrencyById(request.CurrencyId, cancellationToken);
    }
}
