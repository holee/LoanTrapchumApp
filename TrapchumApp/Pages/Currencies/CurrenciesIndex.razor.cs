using Applications.Interfaces.Currencies;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Radzen;

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
                    Skip = pageIndex,
                    PageSize = pageSize,
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
            // return ModalService.Show<SetDailySpotRateModal>(param =>
            // {
            //     param.Add(x => x.onVildated, ValidateSuccess);
            //     param.Add(x => x.OnSuccess, CreateSuccess);

            // }, new ModalInstanceOptions
            //     {
            //         UseModalStructure = false,
            //         FocusTrap = true,
            //         Closing = OnModalClosing,
            //         Size = ModalSize.ExtraLarge
            //     });

            return Task.CompletedTask;
        }


        Task ShowCurrencyDialogue()
        {
            return Task.CompletedTask;
            // return ModalService.Show<ExchangeModalCurrency>(param =>
            // {


            // }, new ModalInstanceOptions
            //     {
            //         UseModalStructure = false,
            //         FocusTrap = true,
            //         Closing = OnModalClosing,
            //         Size = ModalSize.Large
            //     });
        }

    }
}