using Applications.Commons.Mappers.Exchanges;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Applications.Features.Currencies.Command;

public sealed class CreateMoneyExchangeCommandHandler : IRequestHandler<CreateMoneyExchangeCommand, ErrorOr<VttCurrencyExchanges>>
{
    private readonly IForiegnCurrencyExchange _currencyExchangeService;
    public CreateMoneyExchangeCommandHandler(IForiegnCurrencyExchange currencyExchangeService)
    {
        _currencyExchangeService = currencyExchangeService;
    }
    public async Task<ErrorOr<VttCurrencyExchanges>> Handle(CreateMoneyExchangeCommand request, CancellationToken cancellationToken)
    {

        var model = request.Request.ToModel();

        model.TransactionRefId =$"trx-{Guid.CreateVersion7()}";

        return await _currencyExchangeService.CreateExchangeAsync(model, cancellationToken);
    }
}