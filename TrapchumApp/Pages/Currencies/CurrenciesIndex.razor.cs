using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Currencies;

namespace TrapchumApp.Pages.Currencies
{
    public partial class CurrenciesIndex
    {
        private readonly ICurrencyService _currencyService;
        public CurrenciesIndex(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        private PaginatedList<VCurrenciesList> CurrencyPageList = new PaginatedList<VCurrenciesList>();
        private List<VCurrenciesList> Currencies = new List<VCurrenciesList>();
        private int pageIndex = 0;
        private int pageSize = 10;
        private bool includeInactive = false;
        private int count = 0;
        private bool isLoading = false;
        private string filter = "";
        async Task ShowSpotRateDetails()
        {
            //await DetailSpotRateCurrency.InvokeAsync(Currency.CurrencyId);
            await Task.CompletedTask;
        }

        async Task ShowSpotRateCurrency()
        {
            //await EditSpotRateCurrency.InvokeAsync(Currency.CurrencyId);
            await Task.CompletedTask;
        }

        protected override async Task OnInitializedAsync()
        {
            await GetCurrecies();
            await base.OnInitializedAsync();
        }


        async Task GetCurrecies()
        {
            try
            {
                isLoading = true;
                var param = new QueryParam
                {
                    Page = pageIndex,
                    Take = pageSize,
                    SortColumn = "CurrencyCode",
                    SortDescending = false,
                    Filter = filter
                };

                var pageResponse = await _currencyService.GetCurrencies(param);
                Currencies = pageResponse.Items;
                count = pageResponse.Count;
                isLoading = false;
            }
            catch (Exception ex)
            {
                isLoading = false;

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }


        void SpotRateCurrency(int? id)
        {
            NavigationManager.NavigateTo($"/App/Currencies/SpotRate/{id}");
        }
        void ShowCurrencyDetials(int? id)
        {
            NavigationManager.NavigateTo($"/Account/Currencies/{id}");
        }


        Task DisplayCurrencySetSpotRateDialogue()
        {

            return Task.CompletedTask;
        }


        async Task ShowCurrencyDialogue()
        {

           await DialogService.OpenAsync<CurrencyModalCreate>("Currency");

        }

    }
}