using Applications.Interfaces.Transactions;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public sealed class EditMoneyExchangeCommandHandler : IRequestHandler<EditMoneyExchangeCommand, ErrorOr<string>>
{
    private readonly IForiegnCurrencyExchange _currencyExchangeService;
    public EditMoneyExchangeCommandHandler(IForiegnCurrencyExchange currencyExchangeService)
    {
        _currencyExchangeService = currencyExchangeService;
    }
    public async Task<ErrorOr<string>> Handle(EditMoneyExchangeCommand request, CancellationToken cancellationToken)
    {

        var model = request.Request.Adapt<LoanTransaction>();

        return await _currencyExchangeService.EditExchangeAsync(model);
    }
}