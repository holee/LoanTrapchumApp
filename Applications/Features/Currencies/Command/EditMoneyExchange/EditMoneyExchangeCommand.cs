using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using ErrorOr;

namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public record EditMoneyExchangeCommand(ForeignExchangeDtoUpdate Request) : IRequest<ErrorOr<VttCurrencyExchanges>>;
