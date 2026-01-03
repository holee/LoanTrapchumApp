using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Currencies;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetCurrencyDetails;

public sealed class GetCurrencyDetailsByIdQueryHandler : IRequestHandler<GetCurrencyDetailsByIdQuery, ErrorOr<VCurrenciesList>>
{
    private readonly ICurrencyService _currencyRepository;
    public GetCurrencyDetailsByIdQueryHandler(ICurrencyService currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }
    public async Task<ErrorOr<VCurrenciesList>> Handle(GetCurrencyDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetCurrencyDetailById(request.Id, cancellationToken);
        
        if (currency is null)
        {
            return Error.NotFound("NotFound", $"Currency with Id {request.Id} not found.");
        }
        return currency;
    }
}
