using ErrorOr;

namespace Application.Features.Currencies.Command.SetSpotRate;

public record SetSpotRateCommand(int? Id, CurrencySpotRateDtoCreate SpotRates) : IRequest<ErrorOr<SpotRateDtoCreate>>;
