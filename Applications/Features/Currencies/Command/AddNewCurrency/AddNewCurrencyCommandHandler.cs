using Applications.Interfaces.Currencies;
using ErrorOr;
using Mapster;

namespace Applications.Features.Currencies.Command.AddNewCurrency;

public class AddNewCurrencyCommandHandler : IRequestHandler<AddNewCurrencyCommand, ErrorOr<VCurrenciesList>>
{
    private readonly ICurrencyService _currencyService;
    public AddNewCurrencyCommandHandler(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }
    public async Task<ErrorOr<VCurrenciesList>> Handle(AddNewCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = request.Currency.Adapt<Currency>();

        return await _currencyService.SaveOrUpdate(currency);
    }
}