using Applications.Features.Currencies.Responses;
using Applications.Responses.Paginations;
using Microsoft.AspNetCore.Components;
using Radzen;
using Web.UI.Services;

namespace Web.UI.Pages.Currencies
{
    public partial class CurrenciesIndex
    {
        [Inject] private ICurrencyApi CurrencyApi { get; set; } = default!;

        private List<VCurrenciesList> Currencies = new List<VCurrenciesList>();
        private int pageIndex = 0;
        private int pageSize = 10;
        private bool includeInactive = false;
        private int count = 0;
        private bool isLoading = false;
        private string filter = "";

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

                var pageResponse = await CurrencyApi.GetCurrenciesAsync(
                    page: pageIndex,
                    take: pageSize,
                    sortColumn: nameof(VCurrenciesList.CurrencyCode),
                    sortDescending: false,
                    includeInactive: includeInactive,
                    filter: filter);

                Currencies = pageResponse.Items ?? new List<VCurrenciesList>();
                count = pageResponse.Count;
                isLoading = false;
            }
            catch (Exception ex)
            {
                isLoading = false;

                NotificationService.Notify(NotificationSeverity.Error, ex.Message);
            }
        }

        void ShowCurrencyDetials(int? id)
        {
            NavigationManager.NavigateTo($"/Account/Currencies/{id}");
        }

        async Task ShowCurrencyDialogue()
        {
            var created = await DialogService.OpenAsync<CurrencyModalCreate>("Currency");

            if (created is VCurrenciesList)
            {
                await GetCurrecies();
            }
        }
    }
}
