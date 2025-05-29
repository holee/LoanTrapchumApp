using Applications.Enumerations;
using Applications.Interfaces.Currencies;
using Applications.Responses.ViewModels.Currencies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace TrapchumApp.Pages.Transactions.Exchanges
{
    public partial class ExchangeCurrency
    {
        private readonly ICurrencyService _currencyService;

        public ExchangeCurrency(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; } = 0;
        [Parameter, SupplyParameterFromQuery] public string? UserId { get; set; }

        VCurrenciesList? Currency;
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            var user = auth.User;

            var Currencies = await _currencyService.GetCurrencies(string.Empty);

            if (Currencies.Any())
            {
                Currency = Currencies.FirstOrDefault(x => x.CurrencyId == (int)CurrenciesType.Khr);
            }
            await base.OnInitializedAsync();
        }

    }
}