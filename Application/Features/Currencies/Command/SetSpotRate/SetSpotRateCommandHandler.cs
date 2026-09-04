using Application.Interfaces.Transactions;
using ErrorOr;
using Mapster;

namespace Application.Features.Currencies.Command.SetSpotRate;

public sealed class SetSpotRateCommandHandler : IRequestHandler<SetSpotRateCommand, ErrorOr<CurrencyDtoSpotRate>>
{
    private readonly ISpotRateService _spotRateService;
    public SetSpotRateCommandHandler(ISpotRateService spotRateService)
    {
        _spotRateService = spotRateService;
    }
    public async Task<ErrorOr<CurrencyDtoSpotRate>> Handle(SetSpotRateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var spotRates = request.SpotRates.Adapt<CurrencySpotRate>();

            var result = await _spotRateService.SetSpotRate(request.Id, spotRates, cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
