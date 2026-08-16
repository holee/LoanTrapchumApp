using Applications.Features.Currencies.Dto;
using Applications.Features.Currencies.Queries.GetCurrencies;

namespace TrapchumApp.Pages.Transactions.Exchanges
{
    public partial class ExchangeCurrencyIndexPage
    {
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; } = default!;
        [Parameter] public int BranchId { get; set; } = 0;
        private CurrencyDto? Currency;
        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationState;

            var user = auth.User;

            var Currencies = await mediator.Send(new GetCurrenciesQuery((int)CurrenciesType.KHR, new QueryParam(0, 0)));

            if (Currencies.Items.Count > 0)
            {
                Currency = Currencies.Items.FirstOrDefault(x => x.CurrencyId == (int)CurrenciesType.KHR);
            }
            await base.OnInitializedAsync();
        }

    }
}