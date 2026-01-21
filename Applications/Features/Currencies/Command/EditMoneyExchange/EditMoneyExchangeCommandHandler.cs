using Applications.Commons.Mappers.Exchanges;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using ErrorOr;

namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public sealed class EditMoneyExchangeCommandHandler : IRequestHandler<EditMoneyExchangeCommand, ErrorOr<VttCurrencyExchanges>>
{
    private readonly IForiegnCurrencyExchange _currencyExchangeService;
    public EditMoneyExchangeCommandHandler(IForiegnCurrencyExchange currencyExchangeService)
    {
        _currencyExchangeService = currencyExchangeService;
    }
    public async Task<ErrorOr<VttCurrencyExchanges>> Handle(EditMoneyExchangeCommand request, CancellationToken cancellationToken)
    {

        var model = request.Request.ToModel();

        return await _currencyExchangeService.EditExchangeAsync(model, cancellationToken);
    }
}