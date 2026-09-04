using Application.Features.Currencies.Responses;
using Application.Features.Transactions.ForiegnExchanges;
using ErrorOr;

namespace Application.Features.Currencies.Command.EditMoneyExchange;

public record EditMoneyExchangeCommand(ForeignExchangeDtoUpdate Request) : IRequest<ErrorOr<VttCurrencyExchanges>>;
