using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using ErrorOr;

namespace Applications.Features.Currencies.Command;

public record CreateMoneyExchangeCommand(ForeignExchangeDtoCreate Request) : IRequest<ErrorOr<VttCurrencyExchanges>>; 
