using Applications.Interfaces.Transactions;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Applications.Features.Currencies.Command;

public sealed class CreateMoneyExchangeCommandHandler : IRequestHandler<CreateMoneyExchangeCommand, ErrorOr<string>>
{
    private readonly IForiegnCurrencyExchange _currencyExchangeService;
    public CreateMoneyExchangeCommandHandler(IForiegnCurrencyExchange currencyExchangeService) 
    {
        _currencyExchangeService = currencyExchangeService;
    }
    public async Task<ErrorOr<string>> Handle(CreateMoneyExchangeCommand request, CancellationToken cancellationToken)
    {

        var model = request.Request.Adapt<LoanTransaction>();

        return await _currencyExchangeService.CreateExchangeAsync(model);
    }
}