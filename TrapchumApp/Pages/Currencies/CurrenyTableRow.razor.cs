using Applications.Features.Currencies.Responses;

namespace TrapchumApp.Pages.Currencies
{
    public partial class CurrenyTableRow
    {
        [Parameter] public int? RowNumber { get; set; }
        [Parameter] public VCurrenciesList Currency { get; set; } = new();
        [Parameter] public EventCallback<int> EditSpotRateCurrency { get; set; }
        [Parameter] public EventCallback<int> DetailSpotRateCurrency { get; set; }

        async void ShowSpotRateDetails()
        {
            await DetailSpotRateCurrency.InvokeAsync(Currency.CurrencyId);
        }

        async void ShowSpotRateCurrency()
        {
            await EditSpotRateCurrency.InvokeAsync(Currency.CurrencyId);
        }



    }
}