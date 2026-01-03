using Applications.Features.Currencies.Responses;
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
        var result = await _currencyRepository.GetCurrencyById(request.Id);

        if (result is { IsSuccess: false, Data: null })
        {
            return Error.NotFound("NotFound", $"Currency with Id {request.Id} not found.");
        }
        return result.Data!;
    }
}
