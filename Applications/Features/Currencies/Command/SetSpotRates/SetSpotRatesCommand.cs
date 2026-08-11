using ErrorOr;

namespace Applications.Features.Currencies.Command.SetSpotRates;

public record SetSpotRatesCommand(List<CurrencySpotRateDtoCreate> SpotRates) : IRequest<ErrorOr<string>>;
