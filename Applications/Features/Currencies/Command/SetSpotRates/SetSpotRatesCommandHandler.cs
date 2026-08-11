using Applications.Interfaces.Transactions;
using ErrorOr;
using Mapster;

namespace Applications.Features.Currencies.Command.SetSpotRates;

public sealed class SetSpotRatesCommandHandler : IRequestHandler<SetSpotRatesCommand, ErrorOr<string>>
{
    private readonly ISpotRateService _spotRateService;
    public SetSpotRatesCommandHandler(ISpotRateService spotRateService)
    {
        _spotRateService = spotRateService;
    }
    public async Task<ErrorOr<string>> Handle(SetSpotRatesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var spotRates = request.SpotRates.Adapt<List<CurrencySpotRate>>();

            var result = await _spotRateService.SetSpotRates(spotRates, cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
