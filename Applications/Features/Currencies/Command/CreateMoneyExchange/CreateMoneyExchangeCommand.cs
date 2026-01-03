using Applications.Features.Currencies.Requests;
using ErrorOr;

namespace Applications.Features.Currencies.Command;

public record CreateMoneyExchangeCommand(ExchangeDtoCreate Request) : IRequest<ErrorOr<string>>; 
